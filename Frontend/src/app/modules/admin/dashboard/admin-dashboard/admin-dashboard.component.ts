import { Component, ElementRef, HostListener, OnInit, ViewChild, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToasterService } from '../../../../core/services/toaster.service';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels,
  ApexFill,
  ApexGrid,
  ApexStroke,
  ApexTooltip,
  ApexXAxis,
  ApexYAxis,
} from 'ng-apexcharts';
import { DashboardApiService } from '../../../../api-services/dashboard/dashboard-api.service';
import { DashboardSummaryDto, RevenueByDayDto } from '../../../../api-services/dashboard/dashboard-api.models';
import { APP_LOCALE } from '../../../../core/constants/locale';

export type RevenueChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  grid: ApexGrid;
  tooltip: ApexTooltip;
  fill: ApexFill;
  colors: string[];
};

type RangePreset = 'week' | 'month' | 'year' | 'custom';

const DEFAULT_PRESET: 'week' | 'month' | 'year' = 'month';
const MAX_RANGE_DAYS = 730; // matches GetDashboardSummaryQueryValidator on the backend

@Component({
  selector: 'app-admin-dashboard',
  standalone: false,
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss',
})
export class AdminDashboardComponent implements OnInit {
  private dashboardApi = inject(DashboardApiService);
  private router = inject(Router);
  private toaster = inject(ToasterService);

  @ViewChild('filterShell') filterShellRef?: ElementRef<HTMLElement>;

  summary: DashboardSummaryDto | null = null;
  isLoading = true;
  isChartLoading = false;
  loadError = false;

  revenueChartOptions: RevenueChartOptions | null = null;

  // Committed/applied range - what the chart is actually showing.
  rangePreset: RangePreset = DEFAULT_PRESET;
  fromDate = '';
  toDate = '';

  // Draft state inside the open filter panel - only takes effect on "Confirm".
  filterPanelOpen = false;
  draftPreset: RangePreset = DEFAULT_PRESET;
  draftFromDate = '';
  draftToDate = '';

  get todayInputValue(): string {
    return this.toDateInputValue(new Date());
  }

  // Earliest date the pickers allow - keeps anyone from typing in a range the
  // backend would reject anyway, instead of finding out after a failed request.
  get earliestSelectableDate(): string {
    const earliest = new Date();
    earliest.setDate(earliest.getDate() - MAX_RANGE_DAYS);
    return this.toDateInputValue(earliest);
  }

  get maxTopSellingUnits(): number {
    return Math.max(1, ...(this.summary?.topSellingGames.map((g) => g.unitsSold) ?? [1]));
  }

  // revenueByDay is already exactly the applied date range, so this always matches
  // the chart - no separate call needed, it just recomputes whenever summary changes.
  get rangeTotalRevenue(): number {
    return this.summary?.revenueByDay.reduce((sum, day) => sum + day.revenue, 0) ?? 0;
  }

  get activeRangeLabel(): string {
    switch (this.rangePreset) {
      case 'week':
        return 'Past week';
      case 'month':
        return 'Past month';
      case 'year':
        return 'Past year';
      default:
        return `${this.formatShortDate(this.fromDate)} - ${this.formatShortDate(this.toDate)}`;
    }
  }

  ngOnInit(): void {
    const range = this.computePresetRange(DEFAULT_PRESET);
    this.fromDate = range.from;
    this.toDate = range.to;
    this.loadSummary(true);
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.filterPanelOpen) return;

    const target = event.target as Node | null;
    if (target && !this.filterShellRef?.nativeElement.contains(target)) {
      this.filterPanelOpen = false;
    }
  }

  toggleFilterPanel(): void {
    this.filterPanelOpen = !this.filterPanelOpen;

    if (this.filterPanelOpen) {
      // Seed the draft with whatever is currently applied, so reopening the panel
      // doesn't reset or lose the active selection.
      this.draftPreset = this.rangePreset;
      this.draftFromDate = this.fromDate;
      this.draftToDate = this.toDate;
    }
  }

  selectDraftPreset(preset: 'week' | 'month' | 'year'): void {
    this.draftPreset = preset;
    const range = this.computePresetRange(preset);
    this.draftFromDate = range.from;
    this.draftToDate = range.to;
  }

  onDraftDateChange(): void {
    this.draftPreset = 'custom';
  }

  resetFilter(): void {
    this.selectDraftPreset(DEFAULT_PRESET);
  }

  applyFilter(): void {
    if (!this.draftFromDate || !this.draftToDate) {
      return;
    }

    const rangeDays = this.daysBetween(this.draftFromDate, this.draftToDate);
    if (rangeDays > MAX_RANGE_DAYS) {
      this.toaster.error(`Date range can't exceed ${MAX_RANGE_DAYS} days (about 2 years). Please pick a shorter range.`);
      return;
    }

    this.rangePreset = this.draftPreset;
    this.fromDate = this.draftFromDate;
    this.toDate = this.draftToDate;
    this.filterPanelOpen = false;
    this.loadSummary(false);
  }

  loadSummary(isInitial: boolean): void {
    if (isInitial) {
      this.isLoading = true;
      this.loadError = false;
    } else {
      this.isChartLoading = true;
    }

    this.dashboardApi.getSummary({ fromDate: this.fromDate, toDate: this.toDate }).subscribe({
      next: (summary) => {
        this.summary = summary;
        this.revenueChartOptions = this.buildRevenueChartOptions(summary.revenueByDay);
        this.isLoading = false;
        this.isChartLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.isChartLoading = false;

        if (isInitial) {
          // Nothing has ever loaded yet - there's no "last good" dashboard to fall
          // back to, so the full-page error state is the only honest option here.
          this.loadError = true;
          return;
        }

        // A filter-triggered refresh failed - keep showing the dashboard that's
        // already on screen and just surface why the update didn't apply.
        const message = error.error?.message || 'Could not update the chart for that range. Please try again.';
        this.toaster.error(message);
      },
    });
  }

  private daysBetween(fromDate: string, toDate: string): number {
    const msPerDay = 1000 * 60 * 60 * 24;
    return Math.round((new Date(toDate).getTime() - new Date(fromDate).getTime()) / msPerDay);
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat(APP_LOCALE, { style: 'currency', currency: 'EUR' }).format(value);
  }

  formatCompactCurrency(value: number): string {
    return new Intl.NumberFormat(APP_LOCALE, {
      style: 'currency',
      currency: 'EUR',
      notation: 'compact',
      maximumFractionDigits: 1,
    }).format(value);
  }

  formatShortDate(date: Date | string): string {
    const parsed = typeof date === 'string' ? new Date(date) : date;
    return new Intl.DateTimeFormat(APP_LOCALE, { day: '2-digit', month: 'short' }).format(parsed);
  }

  goToOrder(orderId: number): void {
    this.router.navigate(['/admin/orders', orderId, 'details']);
  }

  trackByIndex(index: number): number {
    return index;
  }

  private computePresetRange(preset: 'week' | 'month' | 'year'): { from: string; to: string } {
    const to = new Date();
    const from = new Date();
    const daysBack = preset === 'week' ? 6 : preset === 'month' ? 29 : 364;
    from.setDate(to.getDate() - daysBack);

    return { from: this.toDateInputValue(from), to: this.toDateInputValue(to) };
  }

  private toDateInputValue(date: Date | string): string {
    const parsed = typeof date === 'string' ? new Date(date) : date;
    return parsed.toISOString().slice(0, 10);
  }

  private buildRevenueChartOptions(revenueByDay: RevenueByDayDto[]): RevenueChartOptions {
    const categories = revenueByDay.map((point) => this.formatShortDate(point.date));
    const data = revenueByDay.map((point) => Number(point.revenue.toFixed(2)));

    return {
      series: [{ name: 'Revenue', data }],
      chart: {
        type: 'area',
        height: 260,
        background: 'transparent',
        foreColor: '#8892a4',
        toolbar: { show: false },
        zoom: { enabled: false },
      },
      colors: ['#7ecf43'],
      dataLabels: { enabled: false },
      stroke: { curve: 'smooth', width: 2 },
      fill: {
        type: 'gradient',
        gradient: {
          shadeIntensity: 1,
          opacityFrom: 0.35,
          opacityTo: 0,
          stops: [0, 90, 100],
        },
      },
      grid: {
        borderColor: '#2a2a2a',
        strokeDashArray: 0,
        yaxis: { lines: { show: true } },
        xaxis: { lines: { show: false } },
      },
      xaxis: {
        categories,
        axisBorder: { show: false },
        axisTicks: { show: false },
        // The day-by-day labels overlap into an unreadable mess once the range gets
        // past a couple of weeks - the tooltip already carries the exact date on hover.
        labels: { show: false },
        tooltip: { enabled: false },
      },
      yaxis: {
        labels: {
          style: { colors: '#8892a4' },
          formatter: (val: number) => this.formatCompactCurrency(val),
        },
      },
      tooltip: {
        theme: 'dark',
        y: { formatter: (val: number) => this.formatCurrency(val) },
      },
    };
  }
}
