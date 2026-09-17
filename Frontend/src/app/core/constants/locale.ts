/**
 * Single source of truth for the app's regional formatting.
 * Use this everywhere a date, time, or number needs to be formatted
 * (Angular's date/currency/number pipes, Intl.DateTimeFormat, Intl.NumberFormat,
 * toLocaleDateString, etc.) instead of hardcoding 'en-US', 'en-GB', 'en', etc.
 *
 * Changing the app's regional format only requires updating this file.
 */
export const APP_LOCALE = 'en-GB';
