

import { createContext, useContext, useState } from "react";

const Ctx = createContext(null);
const CHAVE = "inss.sessao";

export function AuthProvider({ children }) {
  const [sessao, setSessao] = useState(() => {
    const salvo = localStorage.getItem(CHAVE);
    return salvo ? JSON.parse(salvo) : null;
  });

  function entrar(dados) {
    localStorage.setItem(CHAVE, JSON.stringify(dados));
    setSessao(dados);
  }
  function sair() {
    localStorage.removeItem(CHAVE);
    setSessao(null);
  }

  return (
    <Ctx.Provider value={{ sessao, entrar, sair }}>
      {children}
    </Ctx.Provider>
  );
}

export function useAuth() {
  return useContext(Ctx);
}
