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
- [Requisiti software e hardware](#requisiti-software-e-hardware)
- [Installazione e avvio](#installazione-e-avvio)
- [Controlli e gameplay](#controlli-e-gameplay)
- [Struttura del progetto](#struttura-del-progetto)
- [Come estendere il progetto](#come-estendere-il-progetto)
- [Test e valutazione](#test-e-valutazione)
- [Problemi noti](#problemi-noti)
- [Licenza e crediti](#licenza-e-crediti)
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

## Requisiti software e hardware
Consigliato:
- Unity Editor: 2021.3 LTS o successivo (aggiorna alla versione LTS che usi nel tuo corso).
- Pacchetti Unity: OpenXR (o il plugin VR che preferisci), XR Interaction Toolkit, TextMeshPro.
- Piattaforma di destinazione: Windows PC per sviluppo; supporto per headset che usano OpenXR (es. Meta Quest tramite Link/ADB, headset SteamVR, Windows Mixed Reality).
- CPU/GPU adeguata per VR; headset VR compatibile.

---

## Installazione e avvio
1. Clona il repository:
   - git clone https://github.com/Alexx904/ECO-SORTER-VR.git
2. Apri Unity Hub e aggiungi il progetto (se necessario, seleziona la versione di Unity raccomandata).
3. Apri il progetto in Unity.
4. Assicurati di installare e abilitare i pacchetti necessari:
   - Window > Package Manager > installa/abilita "XR Interaction Toolkit" e "OpenXR Plugin" (o il plugin VR che preferisci).
5. Configura le impostazioni XR:
   - Project Settings > XR Plug-in Management: abilita il provider adatto (OpenXR).
   - Configura gli action profiles (se richiesto) e i binding per il controller.
6. Apri la scena principale:
   - Apri la scena principale presente in `Assets/Scenes` (es. `Main.unity` o `SampleScene` — verifica il nome esatto nel progetto).
7. Collega l'headset VR o usa il play mode con emulatore (se disponibile) e premi Play in Unity per testare.

Per build standalone:
- File > Build Settings: seleziona la piattaforma (PC, Android per Quest), aggiungi la scena principale e crea la build.

---

## Controlli e gameplay
- Movimento: dipende dall'implementazione XR (teletrasporto o locomozione fisica).
- Interazione: utilizza i controller VR per afferrare (grip/trigger) e rilasciare oggetti.
- Obiettivo: inserire ogni oggetto nel contenitore corretto (es. plastica, vetro, indifferenziato).
- Punteggio: punti assegnati per corretto smaltimento, penalità per errori.
- Timer: (opzionale) sessione a tempo per valutare performance.

(Specifiche esatte dei controlli sono implementate negli script del progetto e nei profili input OpenXR.)

---

## Struttura del progetto (guida rapida)
Cartelle principali (esempi tipici):
- Assets/
  - Scenes/ — scene del gioco
  - Scripts/ — logica di gioco (GameManager, ScoreManager, ObjectBehaviour, VRInteractions)
  - Prefabs/ — oggetti riutilizzabili (rifiuti, contenitori, UI)
  - Art/ — modelli 3D, texture, materiali
  - Audio/ — effetti sonori e musica
  - UI/ — canvas, pannelli HUD
- ProjectSettings/ — impostazioni del progetto (non editarne manualmente se non si sa cosa si fa)

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

## Problemi noti
- Assicurati che i profili OpenXR/Controller siano correttamente configurati; binding mancanti possono impedire le interazioni.
- Performance: ottimizza mesh e texture per ridurre la latenza in VR.

Se trovi un bug, apri una issue nel repository (sezioni "Issues") descrivendo:
- piattaforma e versione Unity
- headset usato
- passaggi per riprodurre
- log/stacktrace (se presente)

---

## Licenza e crediti
- Licenza: MIT (inserire file LICENSE se necessario).
- Asset esterni: verifica singoli asset (modelli, suoni) per eventuali licenze o attribuzioni richieste.

---

## Contatti
Repository: https://github.com/Alexx904/ECO-SORTER-VR  
Autori: Alessandro Miniello, Luca Valenziano — Gruppo 27  
Corso: INGEGNERIA DEL SOFTWARE

Per domande, miglioramenti o integrazioni aprire una issue o contattare via GitHub.

---
