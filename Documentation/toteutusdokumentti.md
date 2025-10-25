# Toteutusdokumentti

## Ohjelman yleisrakenne

- `Program` tarjoaa tekstimuotoisen käyttöliittymän
- `DungeonGenerator` on vastuussa koko generaatioprosessin hallinnasta
- `MapGrid` tekee monia asioita
    - Auttaa varmistamaan, ettei huoneita luodessa tehdä päällekäisiä huoneita
    - Säilyttää lopullista luotua luolastoa Tile-objekteina
    - Lisää luolastoon polunetsinnän löytämät käytävät
    - Hallinnoi Tile-objektien polunetsinnässä käytettäviä cost-arvoja
- `Room` mallintaa huonetta, jolla on muutama ennalta määritelty uloskäynti
- `Edge` mallintaa kaaria huoneiden keskipisteiden välillä
- `Delaunay` sisältää tarvittavaa tietoa ja tarjoaa metodin joka suorittaa Bowyer–Watson algoritmin
    - `Triangle` määrittelee kolmion jota algoritmi ja toteuttaa metodin `InCircumcircle`
- `MinimumSpanningTree` tallentaa tarvittavaa tietoa ja tarjoaa metodin joka suorittaa Kruskalin algoritmin
    - `UnionFind` toteuttaa algoritmin tarvitseman union find tietorakenteen
- `PathFinder` sisältää tarvittua tietoa ja tarjoaa metodin A* polunetsinnän suorittamiseen
    - Löydetty polku tallennetaan suoraan `MapGrid` karttaan


### Generaatioprosessi pähkinänkuoressa

- DungeonGenerator asettaa huoneita MapGridiin, huoneiden keskipisteet tallennetaan
- DungeonGenerator luo Delaunay -instanssin, kutsuu Triangulate-metodia ja tallentaa palautusarvona saamansa kaaret
- DungeonGenerator luo MinimumSpanningTree -instanssin ja kutsuu sen MST-metodia delaunay-kaarilla, tallentaen käytäväkaaret
- DungeonGenerator lisää käytäväkaaria satunnaisesti luodakseen hieman syklisyyttä
- DungeonGenerator kutsuu MapGridin metodia asettaakseen Tile-objekteille sopivat hinnat polunetsintää varten
- DungeonGenerator luo PathFinder -instanssin ja kutsuu sen FindPath-metodia luodakseen käytävän karttaan jokaista käytäväkaarta kohden

## Saavutetut aika- ja tilavaativuudet

### Delaunay Triangulaatio (Bowyer-Watson)
- Toteutunut aikavaativuus on O(n^2) sisäkkäisten silmukoiden vuoksi
- Teoreettisesti olisi ollut mahdollista saavuttaa O(n log n)
- Algoritmi toteutettiin seuraten wikipedian yksinkertaista pseudokoodia
- Tilavaativuus on O(n)
- Suoritusaika on riittävä toivotun kokoisille luolastoille, mutta olisi ongelmallinen suuremman huonemäärän kanssa

### Pienin virittävä puu (Kruskal)
- Aikavaativuus on O(n log n)
- Union-Find operaatioiden aikavaativuus on O(n)
- Tilavaativuus on O(n)

### Polunetsintä (A*)
- Aikavaativuus O(n^2), koska openSet on toteutettu listana eikä prioriteettijonona
- Muuten O(n log n) olisi mahdollinen
- Tällä ei vaikutusta tavoitekokoisten luolastojen generointiin, mutta haittaisi suurempia luodessa
- Tilavaativuus O(n)

## Kehitettävää

- Harjoitustyötä voitaisiin kaikkein selvimmin parantaa parantamalla Bowyer-Watsonin ja A*-polunetsinnän aikavaativuutta.
- Polunetsinnän cost-arvoista ja satunnaisten lisäkäytävien todennäköisyydestä voisi tehdä muokattavia parametreja
- Tekstikäyttöliittymässä on selkeästi parantamisen varaa

Vielä pidemmälle voisi mennä toteuttamalla jo määrittelydokumentissa mainittuja jatkokehitysideoita:
- Huoneiden ja käytävien ulkoasun proseduaalisesti toteuttaminen
- Aarteiden ja vihollisten asettaminen kartalle. Esimerkiksi umpikujiin voisi asettaa aarteita.
- Visuaalisesti vaikuttavampi käyttöliittymä ja mahdollisuus liikuttaa hahmoa kartalla

## Laajojen kielimallien käyttö
Mihin käytetty:
- Käyttöliittymät `Program.cs` sekä `DungeonVisualizerAvaloniaApp` ovat molemmat luotu kokonaan tekoälyä käyttäen
- Kaikki `DebugSnapshotManager.cs` sisältämä koodi sekä suurin osa sitä kutsuvasta koodista on luotu tekoälyllä
- Tiedoston `Prim.cs` sisältämä Primin algoritmin implementaatio on laajalti luotu tekoälyllä. Tätä käytetään vain yksikkötestauksessa
- Apuna XML-documentaation kirjoittamiseen
- Harvoissa tapauksissa neuvon tai mielipiteen kysymiseen. Kaikki muu koodi on silti itse kirjoitettua ja projektin toteutus on käytännössä täysin itse suunniteltu

Käytetyt mallit ovat:
- Claude Sonnet 4.5
- Claude Sonnet 4

Malleja käytettiin VS Coden integroidusta Copilot Chatista

### Käytetyt lähteet

- https://vazgriz.com/119/procedurally-generated-dungeons/
- https://github.com/vazgriz/DungeonGenerator

- https://en.wikipedia.org/wiki/Bowyer%E2%80%93Watson_algorithm
- https://en.wikipedia.org/wiki/Circumcircle#Circumcenter_coordinates

- https://www.youtube.com/watch?v=-L-WgKMFuhE
- https://www.youtube.com/watch?v=mZfyt03LDH4

- https://en.wikipedia.org/wiki/Kruskal%27s_algorithm
- https://raw.githubusercontent.com/hy-tira/tirakirja/master/tirakirja.pdf
- https://www.youtube.com/watch?v=71UQH7Pr9kU