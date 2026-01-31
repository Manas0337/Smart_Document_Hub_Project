import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/Upload.css";

export default function UploadDocument() {
  const [title, setTitle] = useState("");
  const [category, setCategory] = useState("General");
  const [file, setFile] = useState(null);

  const navigate = useNavigate();

  const handleUpload = () => {
    if (!title || !file) {
      alert("Please fill all fields");
      return;
    }

    const newDoc = {
      id: Date.now(),
      title,
      category,
      fileName: file.name,
    };

    const docs =
      JSON.parse(localStorage.getItem("documents")) || [];

    docs.push(newDoc);
    localStorage.setItem("documents", JSON.stringify(docs));

    navigate("/documents");
  };

  return (
    <div className="upload-page">
      {/* BACK BUTTON – SAME AS DOCUMENTS */}
      <div className="back-btn" onClick={() => navigate(-1)}>
        ← Back
      </div>

      {/* TEXT OUTSIDE BOX */}
      <div className="upload-header">
        <h1>Upload & Manage Your Documents</h1>
        <p>
          Securely store, organize and access your important
          files anytime from one place.
        </p>
      </div>

      {/* BIG CARD */}
      <div className="upload-card">
        <input
          type="text"
          placeholder="Document Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />

        <select
          value={category}
          onChange={(e) => setCategory(e.target.value)}
        >
          <option>General</option>
          <option>Personal</option>
          <option>Work</option>
          <option>Finance</option>
        </select>

        <input
          type="file"
          onChange={(e) => setFile(e.target.files[0])}
        />

        <button onClick={handleUpload}>
          Upload Document
        </button>
      </div>
    </div>
  );
}
