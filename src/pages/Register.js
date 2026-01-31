import { useNavigate } from "react-router-dom";
import "../styles/Auth.css";

export default function Register() {
  const navigate = useNavigate();

  const handleRegister = (e) => {
    e.preventDefault();
    navigate("/login");
  };

  return (
    <div className="auth-bg">
      <form className="auth-card" onSubmit={handleRegister}>
        <h2>Create Account</h2>

        <input placeholder="First Name" required />
        <input placeholder="Last Name" required />
        <input type="email" placeholder="Email" required />
        <input type="password" placeholder="Password" required />

        <button>Create Account</button>
      </form>
    </div>
  );
}
