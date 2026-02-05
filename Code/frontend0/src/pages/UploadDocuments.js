import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/UploadDocuments.css";


export default function UploadDocuments() {
  const navigate = useNavigate();

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [type, setType] = useState("Personal");
  const [file, setFile] = useState(null);
  const [loading, setLoading] = useState(false);

  async function uploadDocument(e) {
    e.preventDefault();

    if (!file) {
      alert("Please select a file");
      return;
    }

    try {
      setLoading(true);

      const formData = new FormData();
      formData.append("DocumentName", name);
      formData.append("DocumentDescription", description);
      formData.append("DocumentType", type);
      formData.append("File", file); // 🔥 MUST match backend

      await api("/api/documents", {
        method: "POST",
        body: formData,
      });

      alert("Document uploaded successfully");
      navigate("/documents");
    } catch (err) {
  alert(err?.message || "Upload failed");
  console.error(err);
} finally {
      setLoading(false);
    }
  }

  return (
    <div className="upload-page">
      <button onClick={() => navigate("/dashboard")} className="back-btn">
        ← Back
      </button>

      <h1>Upload & Manage Your Documents</h1>
      <p>Securely store, organize and access your important files anytime.</p>

      <form className="upload-card" onSubmit={uploadDocument}>
        <input
          placeholder="Document Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />

        <textarea
          placeholder="Document Description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
        />

        <select value={type} onChange={(e) => setType(e.target.value)}>
          <option>Personal</option>
          <option>Education</option>
          <option>Work</option>
          <option>Finance</option>
        </select>

        <input
          type="file"
          onChange={(e) => setFile(e.target.files[0])}
          required
        />

        <button type="submit" disabled={loading}>
          {loading ? "Uploading..." : "Upload Document"}
        </button>
      </form>
    </div>
  );
}
