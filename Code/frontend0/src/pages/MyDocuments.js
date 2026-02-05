import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/MyDocuments.css";

export default function MyDocuments() {
  const [documents, setDocuments] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    loadDocuments();
  }, []);

  async function loadDocuments() {
    try {
      const res = await api("/api/documents");
      setDocuments(res || []);
    } catch (err) {
      console.error(err);
      setDocuments([]);
    } finally {
      setLoading(false);
    }
  }

  async function deleteDocument(docId) {
    const confirmed = window.confirm("Are you sure you want to delete this document?");
    if (!confirmed) return;

    try {
      await api(`/api/documents/${docId}`, { method: "DELETE" });
      setDocuments((prev) => prev.filter((doc) => doc.docId !== docId));
    } catch (error) {
      console.error(error);
      alert("Delete failed");
    }
  }

  // ✅ SMART PREVIEW: try text preview first, fallback to file open
  async function previewDocument(doc) {
    const BASE_URL = process.env.REACT_APP_API_BASE_URL || "http://localhost:5102";

    const openFile = () => {
      if (!doc.filePath) {
        alert("No preview available");
        return;
      }

      let path = String(doc.filePath).trim();

      // ensure leading slash
      if (!path.startsWith("/")) path = "/" + path;

      // DB stores "Uploads/..." but static middleware uses "/uploads"
      path = path.replace(/^\/Uploads\//, "/uploads/");

      window.open(`${BASE_URL}${path}`, "_blank", "noopener,noreferrer");
    };

    try {
      // 1) text preview from backend (latest version)
      const res = await api(`/api/documents/${doc.docId}/preview`);

      if (res && res.content) {
        const win = window.open("", "_blank", "noopener,noreferrer");
        win.document.write(`
          <html>
            <head>
              <title>${escapeHtml(res.documentName || "Preview")}</title>
              <style>
                body { font-family: Arial, sans-serif; padding: 24px; line-height: 1.6; }
                pre { white-space: pre-wrap; background:#f7f7f7; padding:16px; border-radius:10px; }
              </style>
            </head>
            <body>
              <h2>${escapeHtml(res.documentName || "Document Preview")}</h2>
              <p><b>Version:</b> ${escapeHtml(String(res.version ?? "-"))}</p>
              <pre>${escapeHtml(String(res.content))}</pre>
            </body>
          </html>
        `);
        win.document.close();
        return;
      }

      // 2) fallback to file preview
      openFile();
    } catch (err) {
      console.error("Text preview failed, opening file preview:", err);
      openFile();
    }
  }

  // prevents HTML injection in preview window
  function escapeHtml(str) {
    return String(str)
      .replaceAll("&", "&amp;")
      .replaceAll("<", "&lt;")
      .replaceAll(">", "&gt;")
      .replaceAll('"', "&quot;")
      .replaceAll("'", "&#039;");
  }

  return (
    <div className="docs-page">
      <div className="docs-header">
        <button onClick={() => navigate("/dashboard")}>← Back</button>
        <h2>📁 My Documents</h2>
        <button onClick={() => navigate("/upload")}>+ Upload</button>
      </div>

      {loading && <p>Loading...</p>}
      {!loading && documents.length === 0 && <p>No documents found</p>}

      {!loading && documents.length > 0 && (
        <div className="docs-list">
          {documents.map((doc) => (
            <div key={doc.docId} className="doc-card">
              <h3>{doc.documentName}</h3>

              <div className="doc-actions">
                <button onClick={() => previewDocument(doc)}>👁 Preview</button>
                <button onClick={() => navigate(`/edit/${doc.docId}`)}>✏ Edit</button>
                <button onClick={() => deleteDocument(doc.docId)}>🗑 Delete</button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
