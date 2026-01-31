import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/Documents.css";

export default function Documents() {
  const [documents, setDocuments] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const stored =
      JSON.parse(localStorage.getItem("documents")) || [];
    setDocuments(stored);
  }, []);

  const deleteDocument = (id) => {
    if (!window.confirm("Delete this document?")) return;

    const updated = documents.filter((d) => d.id !== id);
    localStorage.setItem("documents", JSON.stringify(updated));
    setDocuments(updated);
  };

  const editDocument = (doc) => {
    localStorage.setItem("activeDoc", JSON.stringify(doc));
    navigate("/edit");
  };

  const previewDocument = (doc) => {
    localStorage.setItem("previewDoc", JSON.stringify(doc));
    navigate("/preview");
  };

  return (
    <div className="documents-page">
      {/* BACK BUTTON */}
      <div className="back-btn" onClick={() => navigate(-1)}>
        ← Back
      </div>

      <div className="documents-container">
        {/* HEADER */}
        <div className="documents-header">
          <h2>📁 My Documents</h2>

          <button
            className="primary-btn"
            onClick={() => navigate("/upload")}
          >
            + Upload Document
          </button>
        </div>

        {/* EMPTY STATE */}
        {documents.length === 0 && (
          <div className="empty-state">
            <p>No documents uploaded yet</p>
          </div>
        )}

        {/* DOCUMENT LIST */}
        <div className="documents-list">
          {documents.map((doc) => (
            <div key={doc.id} className="document-card">
              <div className="doc-info">
                <h4>{doc.title}</h4>
                <p className="category">
                  Category: {doc.category}
                </p>
                <span className="filename">
                  {doc.fileName}
                </span>
              </div>

              <div className="doc-actions">
                <button
                  className="action-btn"
                  onClick={() => previewDocument(doc)}
                >
                  👁 Preview
                </button>

                <button
                  className="action-btn"
                  onClick={() => editDocument(doc)}
                >
                  ✏️ Edit
                </button>

                <button
                  className="action-btn danger"
                  onClick={() => deleteDocument(doc.id)}
                >
                  🗑 Delete
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
