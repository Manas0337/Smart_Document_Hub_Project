import { useNavigate } from "react-router-dom";

export default function Preview() {
  const navigate = useNavigate();
  const doc = JSON.parse(localStorage.getItem("previewDoc"));

  if (!doc) return <p>No document found</p>;

  return (
    <div style={{ padding: 30 }}>
      <button onClick={() => navigate(-1)}>← Back</button>

      <h2>{doc.title}</h2>

      {doc.fileType.includes("image") ? (
        <img
          src={doc.fileData}
          alt="preview"
          style={{ maxWidth: "100%" }}
        />
      ) : (
        <iframe
          src={doc.fileData}
          title="preview"
          width="100%"
          height="600px"
        />
      )}
    </div>
  );
}
