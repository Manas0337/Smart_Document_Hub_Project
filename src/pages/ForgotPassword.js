import "../styles/Auth.css";

export default function ForgotPassword() {
  return (
    <div className="auth-bg">
      <div className="auth-card">
        <h2>Forgot Password</h2>
        <input placeholder="Enter registered email" />
        <button>Send Reset Link</button>
      </div>
    </div>
  );
}
