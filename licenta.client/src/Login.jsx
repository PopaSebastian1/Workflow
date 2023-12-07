// Login.js
import React, { useState } from 'react';
import './Login.css'; // Importăm fișierul CSS pentru stilizare

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const handleLogin = () => {
        console.log('Username:', username);
        console.log('Password:', password);

        fetch('https://localhost:7261/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error(`${response.status} ${response.statusText}`);
                }
            })
            .then(data => {
                console.log('Răspuns de la server:', data);
                // Aici poți trata răspunsul și, de exemplu, stoca tokenul în starea componentei sau într-un context global
            })
            .catch(error => {
                console.error('Eroare de rețea sau autentificare:', error.message);
                // Aici poți trata eroarea și, de exemplu, afișa un mesaj de eroare pentru utilizator
            });
    };

    return (
        <div className="login-container">
            <h1>Logare</h1>
            <form>
                <label>
                    Utilizator:
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                </label>
                <br />
                <label>
                    Parolă:
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </label>
                <br />
                <button type="button" onClick={handleLogin}>
                    Autentificare
                </button>
            </form>
        </div>
    );
};

export default Login;
