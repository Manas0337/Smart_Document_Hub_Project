import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/EditDocument.css";

export default function EditDocument() {
  const navigate = useNavigate();

  const [doc, setDoc] = useState(null);
  const [content, setContent] = useState("");
  const [versions, setVersions] = useState([]);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState("");
  const [isEditing, setIsEditing] = useState(false); // ✅ NEW

  // Load active document
  useEffect(() => {
    const stored = JSON.parse(localStorage.getItem("activeDoc"));
    if (!stored) {
      navigate("/documents");
      return;
    }

    setDoc(stored);
    setContent(stored.content || "");
    setVersions(stored.versions || []);
    setComments(stored.comments || []);
  }, [navigate]);

  // Save document
  const saveDocument = () => {
    const updatedDoc = {
      ...doc,
      content,
      versions,
      comments,
    };

    const allDocs =
      JSON.parse(localStorage.getItem("documents")) || [];

    const updatedDocs = allDocs.map((d) =>
      d.id === updatedDoc.id ? updatedDoc : d
    );

    localStorage.setItem("documents", JSON.stringify(updatedDocs));
    localStorage.setItem("activeDoc", JSON.stringify(updatedDoc));

    setIsEditing(false);
    alert("Document saved successfully");
  };

  // Create version
  const createVersion = () => {
    const newVersion = {
      id: Date.now(),
      content,
      date: new Date().toISOString(),
      comments: [...comments],
    };

    setVersions([...versions, newVersion]);
  };

  // Rollback version
  const rollbackVersion = (version) => {
    setContent(version.content);
    setComments(version.comments || []);
  };

  // Add comment
  const addComment = () => {
    if (!newComment.trim()) return;

    const comment = {
      id: Date.now(),
      text: newComment,
      date: new Date().toISOString(),
    };

    setComments([...comments, comment]);
    setNewComment("");
  };

  if (!doc) return null;

  return (
    <div className="editor-page">
      {/* HEADER */}
      <div className="editor-header">
        <button onClick={() => navigate("/documents")}>
          ← Back
        </button>

        <h3>{doc.title}</h3>

        <button
          className="edit-toggle-btn"
          onClick={() => setIsEditing(!isEditing)}
        >
          {isEditing ? "👁 View Mode" : "✏️ Edit Document"}
        </button>
      </div>

      {/* BODY */}
      <div className="editor-body">
        {/* VERSION HISTORY */}
        <div className="card">
          <h4>Version History</h4>

          {versions.length === 0 && (
            <p className="muted-text">No versions yet</p>
          )}

          {versions.map((v, i) => (
            <div
              key={v.id}
              className="version-item"
              onClick={() => rollbackVersion(v)}
            >
              <strong>Version {i + 1}</strong>
              <br />
              <small>
                {new Date(v.date).toLocaleString()}
              </small>
            </div>
          ))}
        </div>

        {/* EDITOR */}
        <div className="card">
          {/* FILE PREVIEW */}
          {doc.fileType?.startsWith("image") && (
            <img
              src={doc.fileData}
              alt="Preview"
              className="doc-preview"
            />
          )}

          {doc.fileType === "application/pdf" && (
            <iframe
              src={doc.fileData}
              title="PDF Preview"
              className="doc-preview"
            />
          )}

          <textarea
            className="editor-textarea"
            value={content}
            disabled={!isEditing}
            onChange={(e) => setContent(e.target.value)}
            placeholder="Edit document content..."
          />
        </div>

        {/* COMMENTS */}
        <div className="card">
          <h4>Comments</h4>

          {comments.length === 0 && (
            <p className="muted-text">No comments yet</p>
          )}

          {comments.map((c) => (
            <div key={c.id} className="comment">
              {c.text}
            </div>
          ))}

          {isEditing && (
            <>
              <textarea
                className="comment-input"
                placeholder="Add comment..."
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
              />

              <button
                className="secondary-btn"
                onClick={addComment}
              >
                Add Comment
              </button>
            </>
          )}
        </div>
      </div>

      {/* FOOTER */}
      {isEditing && (
        <div className="editor-footer">
          <button
            className="secondary-btn"
            onClick={createVersion}
          >
            Create Version
          </button>

          <button
            className="primary-btn"
            onClick={saveDocument}
          >
            Save Changes
          </button>
        </div>
      )}
    </div>
  );
}
