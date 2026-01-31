export default function VersionHistory({ versions, onRestore }) {
  return (
    <div style={{ marginTop: 20 }}>
      <h4>Version History</h4>
      {versions.map((v, i) => (
        <div
          key={i}
          style={{
            padding: 10,
            marginBottom: 8,
            background: "#f1f5f9",
            cursor: "pointer",
          }}
          onClick={() => onRestore(v)}
        >
          Version {i + 1} – {new Date(v.date).toLocaleString()}
        </div>
      ))}
    </div>
  );
}
