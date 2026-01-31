import { useNavigate } from "react-router-dom";
import "../styles/Dashboard.css";

export default function Dashboard() {
  const navigate = useNavigate();
  const user = JSON.parse(localStorage.getItem("user")) || {};

  const handleLogout = () => {
    if (!window.confirm("Are you sure you want to logout?")) return;
    localStorage.removeItem("user");
    navigate("/login");
  };

  return (
    <div className="dashboard">
      {/* HEADER */}
      <div className="dashboard-header">
        <div>
          <h1 className="welcome-text">
            Welcome,
            <span className="user-name">
              {user?.name || "User"}
            </span>{" "}
            👋
          </h1>
          <p className="header-sub">
            Manage your documents smartly & securely
          </p>
        </div>

        <button className="logout-btn" onClick={handleLogout}>
          Logout
        </button>
      </div>

      {/* ACTION BUTTONS */}
      <div className="action-grid">
        <div
          className="action-card primary"
          onClick={() => navigate("/upload")}
        >
          <span className="icon">⬆</span>
          <h4>Upload Document</h4>
          <p>Add a new document from your device</p>
        </div>

        <div
          className="action-card"
          onClick={() => navigate("/documents")}
        >
          <span className="icon">📁</span>
          <h4>My Documents</h4>
          <p>View, edit and manage your files</p>
        </div>
      </div>

      {/* CENTER HIGHLIGHT SECTION */}
      <div className="dashboard-highlight">
        <h2>Smart Document Hub</h2>

        <p className="highlight-subtext">
          A centralized platform designed to securely upload,
          organize, preview, edit and manage all your important
          documents with version control and easy access.
        </p>

        <div className="highlight-features">
          <div className="feature-box">
            <span>📂</span>
            <h4>Centralized Storage</h4>
            <p>
              Store all documents in one secure place with
              structured organization.
            </p>
          </div>

          <div className="feature-box">
            <span>⚡</span>
            <h4>Quick Access</h4>
            <p>
              Instantly preview, edit and rollback document
              versions.
            </p>
          </div>

          <div className="feature-box">
            <span>🔒</span>
            <h4>Secure System</h4>
            <p>
              Ensures privacy, reliability and controlled
              document handling.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
