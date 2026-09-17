export interface TopSellingGameDto {
    gameId: number;
    name: string;
    coverImageURL?: string | null;
    unitsSold: number;
    revenue: number;
}

export interface RecentOrderDto {
    id: number;
    date: string;
    username: string;
    totalAmount: number;
    status: string;
}

export interface RevenueByDayDto {
    date: string;
    revenue: number;
}

export interface GetDashboardSummaryRequest {
    fromDate?: string | null;
    toDate?: string | null;
}

export interface DashboardSummaryDto {
    totalRevenue: number;
    totalOrders: number;
    ordersThisWeek: number;
    totalUsers: number;
    totalGames: number;
    topSellingGames: TopSellingGameDto[];
    recentOrders: RecentOrderDto[];
    revenueByDay: RevenueByDayDto[];
}
