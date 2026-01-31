export default function InlineComments({ comments }) {
  return (
    <div style={{ width: 300, padding: 15, borderLeft: "1px solid #ddd" }}>
      <h4>Comments</h4>
      {comments.length === 0 && <p>No comments yet</p>}
      {comments.map((c, i) => (
        <div key={i} style={{ marginBottom: 10 }}>
          <strong>{c.author}</strong>
          <p>{c.text}</p>
        </div>
      ))}
    </div>
  );
}
