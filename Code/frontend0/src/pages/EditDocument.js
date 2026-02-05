import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import "../styles/EditDocument.css";
import api from "../services/api";

export default function EditDocument() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [viewMode, setViewMode] = useState(false);
  const [content, setContent] = useState("");
  const [latestVersion, setLatestVersion] = useState(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!id) return;

    const loadLatest = async () => {
      try {
        setLoading(true);

        const res = await api(`/api/documents/${id}/versions/latest`);

        if (!res) {
          setLatestVersion(null);
          setContent("");
          return;
        }

        setLatestVersion({
          versionId: res.versionId,
          versionNumber: res.versionNumber,
          time: new Date(res.uploadedAt).toLocaleString(),
        });

        setContent(res.originalText || "");
      } catch (e) {
        console.error(e);
        alert("Failed to load document content");
      } finally {
        setLoading(false);
      }
    };

    loadLatest();
  }, [id]);

  async function saveChanges() {
    if (!id) return;
    if (!content.trim()) {
      alert("Content cannot be empty");
      return;
    }

    try {
      setSaving(true);

      await api(`/api/documents/${id}/versions`, {
        method: "POST",
        body: JSON.stringify({ text: content }),
        headers: { "Content-Type": "application/json" },
      });

      alert("Saved! New version created.");

      // reload latest after save
      const res = await api(`/api/documents/${id}/versions/latest`);
      if (res) {
        setLatestVersion({
          versionId: res.versionId,
          versionNumber: res.versionNumber,
          time: new Date(res.uploadedAt).toLocaleString(),
        });
        setContent(res.originalText || "");
      }
    } catch (e) {
      console.error(e);
      alert("Save failed");
    } finally {
      setSaving(false);
    }
  }

  return (
    <div className="edit-container">
      <div className="edit-header">
        <button className="back-btn" onClick={() => navigate("/documents")}>
          ← Back
        </button>

        <div style={{ textAlign: "center" }}>
          <h2 style={{ margin: 0 }}>Document Editor</h2>
          {latestVersion && (
            <small style={{ opacity: 0.8 }}>
              Version {latestVersion.versionNumber} • {latestVersion.time}
            </small>
          )}
        </div>

        <button className="view-btn" onClick={() => setViewMode(!viewMode)}>
          👁 {viewMode ? "Edit Mode" : "View Mode"}
        </button>
      </div>

      <div className="edit-body">
        <div className="editor-panel" style={{ width: "100%" }}>
          {loading ? (
            <p>Loading...</p>
          ) : (
            <textarea
              value={content}
              onChange={(e) => setContent(e.target.value)}
              readOnly={viewMode}
              placeholder="Edit document content..."
              style={{ width: "100%" }}
            />
          )}
        </div>
      </div>

      <div className="edit-footer">
        <button
          className="save-btn"
          onClick={saveChanges}
          disabled={loading || saving}
        >
          {saving ? "Saving..." : "Save Changes (Create Version)"}
        </button>
      </div>
    </div>
  );
}
