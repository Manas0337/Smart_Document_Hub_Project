import { useState } from "react";
import api from "../services/api";
import { useNavigate, Link } from "react-router-dom";
import "../styles/Registration.css";

export default function Register() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  async function handleSubmit(e) {
    e.preventDefault();

    try {
      await api("/api/auth/register", {
        method: "POST",
        body: JSON.stringify({ name, email, password }),
      });

      alert("Registered successfully. Please login.");
      navigate("/");
    } catch (err) {
  alert(err?.message || "Registration failed");
  console.error(err);
}

  }

  return (
    <div className="register-page">
      <div className="register-card">
        <h2 className="register-title">Smart Document Hub</h2>
        <p className="register-subtitle">
          Manage and organize your documents
        </p>

        <form className="register-form" onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />

          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />

          <button type="submit">Register</button>
        </form>

        <div className="register-links">
          <Link to="/">Back to login</Link>
        </div>
      </div>
    </div>
  );
}
