import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import "../styles/Auth.css";

export default function Login() {
  const [password, setPassword] = useState("");
  const [error, setError] = useState(false);
  const navigate = useNavigate();

  const handleLogin = () => {
    if (password === "admin123") {
      localStorage.setItem("auth", "true");
      navigate("/dashboard");
    } else {
      setError(true);
    }
  };

  return (
    <div className="auth-bg">
      <div className="auth-card">
        <h2>Smart Document Hub</h2>
        <p className="subtitle">Manage and organize your documents</p>

        <input placeholder="First Name" />
        <input placeholder="Last Name" />
        <input placeholder="Email" />
        <input
          type="password"
          placeholder="Password"
          onChange={(e) => setPassword(e.target.value)}
        />

        {error && <p className="error">Incorrect password</p>}
        {error && <Link to="/forgot">Forgot Password?</Link>}

        <button onClick={handleLogin}>Login</button>
      </div>
    </div>
  );
}
