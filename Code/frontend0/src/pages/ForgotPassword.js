import { useState } from "react";
import api from "../services/api";
import { Link } from "react-router-dom";
import "../styles/ForgotPassword.css";

export default function ForgotPassword() {
  const [email, setEmail] = useState("");
  const [msg, setMsg] = useState("");

  async function handleSubmit(e) {
    e.preventDefault();

    try {
      const res = await api("/api/auth/forgot-password", {
        method: "POST",
        body: JSON.stringify({ email }),
        headers: {
          "Content-Type": "application/json",
        },
      });

      setMsg(res?.message || "Reset link sent to your email");
    } catch (err) {
      console.error(err);
      setMsg("Email does not exist");
    }
  }

  return (
    <div className="forgot-page">
      <div className="forgot-card">
        <h2 className="forgot-title">Smart Document Hub</h2>
        <p className="forgot-subtitle">
          Enter your email to reset your password
        </p>

        <form className="forgot-form" onSubmit={handleSubmit}>
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <button type="submit">Send Link</button>
        </form>

        {msg && <p className="forgot-message">{msg}</p>}

        <div className="forgot-links">
          <Link to="/">Back to login</Link>
        </div>
      </div>
    </div>
  );
}
