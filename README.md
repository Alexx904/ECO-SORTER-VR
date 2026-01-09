# ECO-SORTER-VR

Progetto Unity realizzato per l'esame universitario "INGEGNERIA DEL SOFTWARE".  
Simulazione VR per l'apprendimento e la pratica del corretto smaltimento dei rifiuti: il giocatore osserva/afferra oggetti e li smista nei contenitori corretti per guadagnare punti.

Gruppo 27 — Componenti:
- Alessandro Miniello
- Luca Valenziano

---

## Indice
- [Descrizione](#descrizione)
- [Caratteristiche principali](#caratteristiche-principali)
- [Controlli e gameplay](#controlli-e-gameplay)
- [Struttura del progetto](#struttura-del-progetto)
- [Come estendere il progetto](#come-estendere-il-progetto)
- [Test e valutazione](#test-e-valutazione)
- [Contatti](#contatti)

---

## Descrizione
ECO-SORTER-VR è una applicazione didattica in realtà virtuale che mette il giocatore nella situazione di dover smistare correttamente rifiuti di vario tipo. L'obiettivo è promuovere buone pratiche ambientali in un ambiente immersivo, e allo stesso tempo dimostrare l'applicazione di principi di ingegneria del software nella progettazione del gioco (architettura, modularità, testabilità).

---

## Caratteristiche principali
- Esperienza in Realtà Virtuale (VR).
- Meccaniche di presa/rilascio oggetti e smistamento in contenitori dedicati.
- Sistema di punteggio e feedback (audio/visuale).
- Interfaccia utente in-game con timer e contatore punti.
- Scene e prefab modulari per facile estensione.

---

## Controlli e gameplay
- Movimento: Levetta sinistra del Meta Quest.
- Interazione: utilizza i controller VR per afferrare (grip/trigger) e rilasciare oggetti.
- Obiettivo: inserire ogni oggetto nel contenitore corretto (es. plastica, vetro, indifferenziato).
- Punteggio: punti assegnati per corretto smaltimento, penalità per errori.
- Timer: sessione a tempo per valutare performance.


---

## Struttura del progetto (guida rapida)
Cartelle principali:
- Assets/
  - Scenes/ — scene del gioco (LIvelli)
  - Scripts/ — logica di gioco (codice in C#)
  - Prefabs/ — oggetti riutilizzabili (rifiuti, contenitori, UI)
  - Models Scaricati/ — Assets scarricati dall'Unity Assets Store
  - Audio/ — effetti sonori e musica
  

---

## Come estendere il progetto
- Aggiungere nuovi tipi di rifiuto: creare un prefab e configurare il suo tag/categoria, aggiornare la logica di valutazione.
- Nuove modalità di gioco: modalità a tempo, modalità libera, livelli con difficoltà crescente.
- Analytics: salvare punteggi locali o inviarli a un server per classifiche.
- Localizzazione: tradurre testi UI e messaggi.

---

## Test e valutazione
- Test funzionali: verificare che ogni oggetto venga riconosciuto correttamente quando inserito in un contenitore.
- Test VR: test su device reale per latenza, comfort e corretto binding dei controller.
- Test di regressione: ogni modifica agli script principali dovrebbe essere testata nelle scene che usano quei sistemi.


---

## Contatti
Repository: https://github.com/Alexx904/ECO-SORTER-VR  
Autori: Alessandro Miniello, Luca Valenziano — Gruppo 27  
Corso: INGEGNERIA DEL SOFTWARE

Per domande, miglioramenti o integrazioni aprire una issue o contattare via GitHub.

---
