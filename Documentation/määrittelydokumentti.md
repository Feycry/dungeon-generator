Opinto-ohjelma: tietojenkäsittelytieteen kandidaatti
Projektin kieli: tulen kommentoimaan koodia englanniksi. Toteutusdokumentin ja testausdokumentin kirjoitan todennäköisesti myös englanniksi.

### Mitä ohjelmointikieltä käytät?

Käytän projektin toteutuksessa C#-kieltä, jotta voin hyödyntää koodia myöhemmin omassa peliprojektissa.

### Kerro myös mitä muita kieliä hallitset siinä määrin, että pystyt tarvittaessa vertaisarvioimaan niillä tehtyjä projekteja.

Python ja C++.

### Mitä algoritmeja ja tietorakenteita toteutat työssäsi?

##### Tietorakenteita:
- Jokin yksinkertainen kaksiulotteinen taulukko luolaston säilyttämiseen

##### Algoritmeja:
- Jokin yksinkertainen itse keksitty satunnaisalgoritmi huoneiden luomiseen joka hyödyntää satunnaislukuja ja normaalijakaumaa. Huoneiden leveys päätetään normaalijakauman mukaan. Satunnaislukujen generointia en ajatellut koodata itse. Päällekkäisistä huoneista poistetaan toinen.
- Delaunay triangulaatio, luultavasti Bowyer-Watson
- Pienimmän virittävän puun (minimum spanning tree) löytäminen, luultavasti Kruskalin algoritmilla
- A* huoneiden välisten käytävien luomiseen

### Minkä ongelman ratkaiset?

Ongelmana on kaksiulotteisen luolaston / labyrintin / pelikartan luominen niin, että se on halutun kokoinenen.

Lisäksi käyttäjän tulee pystyä määrittelemään lista koordinaatteja, joihin on ainakin luotava huone. Täten voidaan varmistaa, että kartasta löytyy esimerkiksi alareunasta aloitushuone ja yläreunasta maalihuone, joiden kautta luotu kartta voidaan yhdistää osaksi laajempaa pelimaailmaa.

Projektin ydin on kolmen käytettävän algoritmin implementointi: Delaunay triangulaatio, pienimmän virittävän puun löytäminen ja reitinetsintä.

### Suunnitelma kartan luontiin:
- Luo huoneita
- Toteuta huoneiden välille Delaunay triangulaatio
- Laske pienin virittävä puu triangulaatiosta
- Luo pienimmän virittävän puun mukaiset käytävät huoneiden välille A* polunetsinnällä, rangaisten huoneiden läpi kulkemista
- Lisää satunnaisesti (esim. 10% todennäköisyydellä) joidenkin triangulaatiossa luotujen kaarien välille myös käytävä, vaikka ne eivät olisi osa pienintä virittävää puuta


### Mitä syötteitä ohjelma saa ja miten niitä käytetään?

Kartan koordinaatiston origo on vasemmassa yläkulmassa. Täten mitä suurempi y:n arvo on, sitä alempana sijainti.
##### Syötteet:
- Kartan leveys, esim. 100 solua
- Kartan korkeus, esim. 150 solua
- Siemenluku (seed) satunnaisgenerointia varten

##### Vapaavalinnaiset syötteet:
- Lista huoneita, jotka aina generoidaan. Jokainen listassa oleva huone on tuple muotoa (x, y, leveys, korkeus). Lista voi olla tyhjä. Kartan ulkopuolelle (edes osittain) sijoittuvat huoneet johtavat virhetulosteeseen.
	- x on huoneen vasemman laidan koordinaatti
	- y on huoneen ylälaidan koordinaatti
	- leveys on huoneen leveys soluina
	- korkeus on huoneen korkeus soluina
- Huoneiden määrä, oletusarvo määräytyy lukuna suhteessa kartan kokoon (esim. floor(0.1*(leveys+korkeus)))
- Huoneen sivun pituuden minimi, oletusarvo 1 solua
- Huoneen sivun pituuden maksimi, oletusarvo 9 solua
- Huoneiden koon määrittävän normaalijakauman odotusarvo, oletusarvona 4 (solua)
- Huoneiden koon määrittävän normaalijakauman varianssi, oletusarvona 1.6


### Käyttö ja tulostus:

Minimitavoitteena on että erillinen moduuli pystyy piirtämään kohtuullisen kokoisen (esim. 100 x 100) luodun kartan tekstitulosteena.

Toiveena olisi siis, että projektia voi käyttää myöhemmin kirjastona, mutta kurssia varten luodaan yksinkertainen tekstikäyttöliittymä.

### Tavoitteena olevat aika- ja tilavaativuudet:

Algoritmi tullaan oikeassa käytössä luultavasti suorittamaan vain kerran pelin alussa, joten riittää kunhan se suoritettaisiin esimerkiksi sekunnissa. Realistisen kokoisilla kartoilla (500 x 500) ei pitäisi olla mitään ongelmaa suoritusajan suhteen.

n = huoneiden määrä
l = kerkiverto käytävän pituus

Huoneiden määrä on aina huomattavasti pienempi kuin kartan koko w * h.

##### Osittain:
- Huoneiden asettaminen O(n)
- Bowyer-Watson O(n log n)
- Kruskal O(n log n) (rajoittava tekijä on kaarien järjestäminen)
- A* O(n*l)

Yhteensä O(n log n + n*l)

Tilavaativuus määräytyy lähinnä kaksiulotteisen taulukon säilyttämiseen, eli O(w*h), missä w on kartan leveys ja h korkeus. Algoritmien tilantarve taitaa olla riippuvainen huoneiden määrästä oli O(n). Täten koko generoinnin pitäisi vaatia enintää O(w*h + n).

Lopullinen kokonaisuus ei tule siis soveltumaan hyvin valtavien karttojen (esim. 10000 x 10000) luomiseen, sillä suurimmassa osassa tapauksessa leveys ja korkeus ovat samaa suuruusluokkaa ja tulavaativuus kasvaa neliömäisesti.

### Projektin rakenne:

Laaja kokonaisuus tulee implementoida moneen eri tiedostoon. Suunnitelma voi helposti muuttua, mutta tarvitaan ainakin:
- DungeonGenerator.cs
- MapGrid.cs
- MSTKruskal.cs
- DelaunayTriangulation.cs
- AStar.cs
- Interface.cs

Joidenkin algoritmien testausta ja kehittänmistä varten olisi hyvä olla jokinlainen visualisointi. Tämän voisi toteuttaa esimerkiksi tekoälyllä.

### Lähteet, joita aiot käyttää:

Lista ei ole tietenkään kattava vielä tässä vaiheessa, mutta käytin näitä aiheeseen tutustuessa.

- https://en.wikipedia.org/wiki/Bowyer%E2%80%93Watson_algorithm
- https://en.wikipedia.org/wiki/Kruskal%27s_algorithm
- https://en.wikipedia.org/wiki/A*_search_algorithm#Complexity

- https://stackoverflow.com/questions/218060/random-gaussian-variables
- https://vazgriz.com/119/procedurally-generated-dungeons/
- https://github.com/vazgriz/DungeonGenerator
- https://www.reddit.com/r/gamedev/comments/1dlwc4/procedural_dungeon_generation_algorithm_explained/

- https://www.youtube.com/watch?v=-L-WgKMFuhE
- https://www.youtube.com/watch?v=mZfyt03LDH4

- https://raw.githubusercontent.com/hy-tira/tirakirja/master/tirakirja.pdf

- https://www.youtube.com/watch?v=71UQH7Pr9kU
- https://www.youtube.com/watch?v=GctAunEuHt4

### Laajennusideoita:

Tässä muutama laajennusidea joita projektiin voisi aikaa jäädessä implementoida.

- Huoneiden ja käytävien ulkoasun proseduaalisesti toteuttaminen
- Aarteiden ja vihollisten asettaminen kartalle. Esimerkiksi umpikujiin voisi asettaa aarteita.
- Visuaalisesti vaikuttavampi käyttöliittymä ja mahdollisuus liikuttaa hahmoa kartalla