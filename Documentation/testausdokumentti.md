# Testausdokumentti

Automaattiset yksikkötestit sijaitsevat `DungeonGeneratorTest`-projektissa.

**Arvioitu testauskattavuus ~60%** (Pois lukien debug koodin ja käyttöliittymät)

Testejä on yhteensä 21, joista 18 menee läpi onnistuneesti. Epäonnistuneet testit liittyvät Delaunay-triangulaation reunatapauksiin, jotka eivät vaikuta ohjelman normaaliin käyttöön luolastojen generoinnissa.

Testit keskittyvät erityisesti ydinalgoritmeille (Delaunay, MST, A*), joiden kattavuus on lähes 100%. Muuta toiminnallisuutta testataan end-to-end testeillä.

## Mitä on testattu?
- Kaikkia kolmea keskeistä algoritmia testattiin, sekä generaatiota alusta loppuun. Testatut algoritmit ovat:
    - Bowyer-Watson
    - Kruskal
    - A*

## Luettelo testeistä ja syötteistä

### 1. Delaunay-triangulaatio (Bowyer-Watson algoritmi)

`DelaunayTest.cs`

- **TestTriangulation**: Testaa neliön triangulaatiota
  - Syöte: Pisteet (0,0), (10,0), (10,10), (0,10)
  - Odotettu tulos: 5 kaarta
  - Tulos: toimii

- **TestTriangleTriangulation**: Testaa kolmion muodostumista kolmella pisteellä
  - Syöte: Pisteet (0,0), (10,0), (5,10)
  - Odotettu tulos: 3 kaarta, kaikki 3 pistettä yhdistetty
  - Tulos: toimii

- **TestCollinearPoints**: Testaa kollineaarisia pisteitä
  - Syöte: Pisteet samalla vaakatasolla (0,0), (5,0), (10,0), (15,0)
  - Odotettu tulos: Jokin triangulaatio
  - Tulos: epäonnistuu

- **TestInsufficientPoints**: Testaa liian vähän pisteitä
  - Syöte: 2 pistettä (0,0), (10,10)
  - Odotettu tulos: ArgumentException
  - Tulos: toimii

- **TestNullInput**: Testaa null-syötettä
  - Syöte: null
  - Odotettu tulos: ArgumentException
  - Tulos: toimii

- **TestDuplicatePoints**: Testaa duplikaattipisteiden käsittelyä
  - Syöte: Pisteet joissa kaksi identtistä
  - Odotettu tulos: Vähintään 1 kaari
  - Tulos: toimii

- **TestLargeCoordinates**: Testaa suuria koordinaatteja
  - Syöte: Pisteet (1000000,2000000), (1000010,2000000), (1000005,2000010)
  - Odotettu tulos: 3 kaarta
  - Tulos: toimii

- **TestNarrowCoordinates**: Testaa kapeita koordinaatteja
  - Syöte: Pisteet (1,2000000), (3,2000000), (2,2000010)
  - Odotettu tulos: 3 kaarta
  - Tulos: toimii

- **TestWideCoordinates**: Testaa leveitä koordinaatteja
  - Syöte: Pisteet (1000000,3), (1000010,1), (1000005,2)
  - Odotettu tulos: 3 kaarta
  - Tulos: epäonnistuu

- **TestNegativeCoordinates**: Testaa negatiivisia koordinaatteja
  - Syöte: Pisteet (-10,-10), (0,-5), (-5,5), (10,10)
  - Odotettu tulos: Toimiva triangulaatio, vähintään 3 kaarta
  - Tulos: epäonnistuu

### 2. Minimum Spanning Tree (Kruskal)

`KruskalTest.cs`

- **TestKruskalUnionFind**: Testaa UnionFind-tietorakenteen toimintaa
  - Syöte: 3 solmua (0,0), (1,1), (2,2)
  - Odotettu tulos: Find toimii oikein, Union yhdistää solmut
  - Tulos: toimii

- **TestKruskalSimple**: Testaa Kruskalin ja Primin algoritmien yhtäpitävyyttä
  - Syöte: 3 solmua kolmiossa
  - Odotettu tulos: Kruskal ja Prim tuottavat saman pienimmän virittävän puun
  - Tulos: toimii

- **TestKruskalRandom**: Testaa suurella satunnaissyötteellä
  - Syöte: 200 satunnaista solmua, satunnaisia kaaria
  - Odotettu tulos: Kruskal ja Prim tuottavat saman painoisen pienimmän virittävän puun
  - Tulos: toimii

### 3. Polunetsintä (A*)

`PathFinderTest.cs`

- **TestLargeRandomGrid**: Testaa polunetsintää suurilla satunnaisilla kartoilla
  - Syöte: 100 iteraatiota, kartat kooltaan 50x50 - 200x200, satunnaiset hinnat ja lähtö/maalipisteet
  - Odotettu tulos: Polku löytyy jokaisella kierroksella
  - Tulos: toimii

### 4. End-to-End testaus

`EndToEndTest.cs`

- **TestCompleteDungeonGeneration**: Testaa koko luolaston generointiprosessin
  - Syöte: 30x30 kartta, seed 12345, allowDiagonals=false, 1 kiinteä huone (5,5,4,4), 10 satunnaista huonetta
  - Odotettu tulos: Kartan koko täsmää, kiinteä huone on olemassa, vähintään 16
  - Tulos: toimii

- **TestDungeonGenerationSmall**: Testaa pienempää luolastoa kolmella kiinteällä huoneella
  - Syöte: 15x15 kartta, seed 22222, allowDiagonals=false, 3 kiinteää huonetta, 0 satunnaista huonetta
  - Odotettu tulos: Kartan koko täsmää, kaikki kiinteät huoneet olemassa
  - Tulos: toimii


## Empiirinen testaus

Ohjelman toimintaa on testattu myös käytännössä generoimalla lukuisia erilaisia luolastoja eri parametreilla käyttäen visuaalista käyttöliittymää.

## Visuaalinen testaus

Implementoitu myös DebugSnapshotManager, joka tallentaa generointiprosessin vaiheita PNG-kuvina (`step_000.png` - `step_005.png`). Lisäksi vusiaalisesta käyttöliittymästä voi nähdä, että generaatio tapahtuu oletetulla tavalla.

## Testien toistaminen

Kaikki testit voi pyörittää projektin juurikansiosta komennolla:
```
dotnet test DungeonGeneratorTest
```