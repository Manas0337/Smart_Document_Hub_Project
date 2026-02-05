// api.js

// Hardcoded backend URL
const BASE_URL = "http://localhost:5102";

/**
 * Generic API helper
 * @param {string} endpoint - API endpoint, e.g., "/api/auth/register"
 * @param {object} options - fetch options (method, body, headers)
 * @returns {Promise<any>}
 */
export default async function api(endpoint, options = {}) {
  const token = localStorage.getItem("token");
  const isFormData = options.body instanceof FormData;

  const headers = {
    ...(isFormData ? {} : { "Content-Type": "application/json" }),
    ...(options.headers || {}),
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
  };

  try {
    const res = await fetch(`${BASE_URL}${endpoint}`, {
      ...options,
      headers,
    });

    // Handle HTTP errors
    if (!res.ok) {
      const errorText = await res.text().catch(() => "");
      console.error("API call failed:", endpoint, res.status, errorText);
      throw new Error(errorText || `API Error: ${res.status}`);
    }

    // No content
    if (res.status === 204) return null;

    // Parse JSON if available, otherwise return text
    const contentType = res.headers.get("content-type") || "";
    if (contentType.includes("application/json")) {
      return res.json();
    }

    return res.text();
  } catch (err) {
    console.error("Fetch failed (is backend running?):", err);
    throw new Error("Unable to connect to backend. Make sure the server is running.");
  }
}
