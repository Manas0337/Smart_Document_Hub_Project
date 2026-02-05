import { useState } from "react";
import api from "../services/api";
import { useSearchParams, useNavigate } from "react-router-dom";

export default function ResetPassword() {
  const [password, setPassword] = useState("");
  const [params] = useSearchParams();
  const navigate = useNavigate();
  const token = params.get("token");

 async function handleSubmit(e) {
  e.preventDefault();

  try {
    const res = await api("/api/auth/reset-password", {
      method: "POST",
      body: JSON.stringify({
        token,
        newPassword: password,
      }),
      headers: {
        "Content-Type": "application/json",
      },
    });

    alert(res.message);
    navigate("/");
  } catch (err) {
    console.error(err);
    alert("Reset failed");
  }
}


  return (
    <div className="card">
      <h2>Reset Password</h2>

      <form onSubmit={handleSubmit}>
        <input placeholder="New Password" type="password" value={password} onChange={e => setPassword(e.target.value)} />
        <button>Reset</button>
      </form>
    </div>
  );
}
