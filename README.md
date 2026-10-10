# Othello (Reversi) – Unity Game

## Opis projektu
Othello (znane również jako Reversi) to klasyczna gra planszowa dla dwóch graczy. Celem gry jest zdobycie większej liczby pionków swojego koloru niż przeciwnik po zapełnieniu planszy lub gdy żaden z graczy nie może wykonać ruchu.

Projekt został zrealizowany w silniku **Unity** i stanowi implementację cyfrowej wersji gry Othello z intuicyjnym interfejsem oraz logiką zgodną z oficjalnymi zasadami gry.

## Zasady gry
- Gra toczy się na planszy **8×8**.
- Gracze wykonują ruchy naprzemiennie (czarny zaczyna).
- Ruch jest dozwolony tylko wtedy, gdy powoduje „zamknięcie” co najmniej jednego pionka przeciwnika w linii poziomej, pionowej lub ukośnej.
- Wszystkie zamknięte pionki przeciwnika zmieniają kolor na kolor gracza wykonującego ruch.
- Gra kończy się, gdy:
  - plansza jest pełna lub
  - żaden z graczy nie może wykonać poprawnego ruchu.

## Funkcjonalności
- Plansza 8×8 generowana dynamicznie
- Pełna logika zasad gry Othello
- Walidacja poprawnych ruchów
- Automatyczne odwracanie pionków
- Licznik punktów dla obu graczy
- Informacja o aktualnym graczu
- Wykrywanie końca gry i zwycięzcy

## Technologie
- **Unity** (silnik gry)
- **C#** (logika gry)
- Unity UI (Canvas, Button, Text / TMP)
