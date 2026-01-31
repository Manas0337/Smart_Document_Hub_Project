export default function DocumentEditor({ content, onChange }) {
  return (
    <textarea
      style={{
        width: "100%",
        height: "70vh",
        padding: 20,
        fontSize: 16,
      }}
      value={content}
      onChange={(e) => onChange(e.target.value)}
      placeholder="Start editing document..."
    />
  );
}
