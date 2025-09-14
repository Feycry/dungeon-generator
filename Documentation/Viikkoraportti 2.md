Viikko 2. Käytetty aika noin 8 tuntia.


### Mitä olen tehnyt tällä viikolla?

Olen käyttänyt viikon tukevan pohjan luomiseen projektille. Loin tekoälyn avulla käyttöliittymäpohjan (Program.cs) ja verkkojen piirtämismoduulin (Visualize.cs), jonka avulla voin selvitellä ongelmia jatkossa verkkoalgoritmeja luodessa. Lisäksi koodasin itse luokat DungeonGenerator ja MapGrid. Yksikkötestejä en ole vielä ehtinyt luomaan.

### Miten ohjelma on edistynyt?

Tällä hetkellä ohjelma pystyy vain asettamaan huoneita kartalle ja tulostamaan kartan tekstinä.

### Mitä opin tällä viikolla / tänään?

Box-Muller transform oli uusi.

### Mikä jäi epäselväksi tai tuottanut vaikeuksia?

- Seuraavaksi pitäisi päästä keskittymään projektin ytimeen. Verkkoalgoritmeja varten tarvitsen huoneiden keskipisteet, jotka voidaan laskea rooms-listasta vähentämällä puolet leveydestä ja puolet korkeudesta x ja y koordinaateista. Haittaakohan, jos osa pisteistä on tällöin esim. 24.5 eikä kokonaislukuja. Veikkaan että ei haittaa.
- Pitäisiköhän huoneille luoda oma luokka, joka tallentaa myös keskipisteen? Ei ehkä jos keskipisteet lasketaan vain kerran. Tällä hetkellä huoneet ovat vain tupleja.

### Mitä teen seuraavaksi?

Luon jonkin yksikkötestin huoneiden asettamiselle ja toteutan ainakin yhden verkkoalgoritmeista.