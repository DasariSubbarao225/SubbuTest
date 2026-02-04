import React from "react";

export default function Home() {
  return (
    <main style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: '100vh', fontFamily: 'sans-serif' }}>
      <h1 style={{ fontSize: '2.5rem', marginBottom: '1rem', color: '#715837' }}>Weaver Interiors</h1>
      <p style={{ fontSize: '1.35rem', color: '#466353', marginBottom: '2rem' }}>
        Transforming Spaces with Elegance
      </p>
      <p style={{ color: '#555', fontSize: '1rem', maxWidth: 420, textAlign: 'center' }}>
        Welcome to Weaver Interiors — your partner for premium home interiors, woodwork, and bespoke solutions. Explore our portfolio and discover how we can turn your vision into reality.
      </p>
    </main>
  );
}