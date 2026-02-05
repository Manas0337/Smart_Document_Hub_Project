import { useState } from "react";
import api from "../services/api";   
import { useNavigate, Link } from "react-router-dom";
import "../styles/Login.css";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");

    try {
      const res = await api("/api/auth/login", {
        method: "POST",
        body: JSON.stringify({ email, password }),
        headers: {
          "Content-Type": "application/json",
        },
      });

      localStorage.setItem("token", res.token);
      localStorage.setItem("email", res.email);
      localStorage.setItem("userId", res.userId);

      navigate("/dashboard");
    } catch (err) {
      setError(err.message);
    }
  }

  return (
    <div className="login-page">
      <div className="login-overlay">
        <div className="login-card">
          <h2>Smart Document Hub</h2>
          <p className="subtitle">Manage and organize your documents</p>

          <form onSubmit={handleSubmit}>
            <input
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />

            <input
              placeholder="Password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />

            <button type="submit">Login</button>
          </form>

          {error && <p className="error">{error}</p>}

          <div className="links">
            <Link to="/forgot">Forgot password?</Link>
            <Link to="/register">Create account</Link>
          </div>
        </div>
      </div>
    </div>
  );
}
