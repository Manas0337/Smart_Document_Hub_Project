import { apiRequest } from "./api";

export const getDashboardStats = () =>
  apiRequest("/api/dashboard/stats");

export const getRecentActivity = () =>
  apiRequest("/api/dashboard/activity");
