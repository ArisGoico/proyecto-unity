﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TiposSeres : MonoBehaviour {
	
	//Tipos de edificios
	private TipoEdificio fabricaComBas;
	private TipoEdificio energia;
	private TipoEdificio granja;
	private TipoEdificio fabricaComAdv;
	private TipoEdificio energiaAdv;
	
	//Tipos de animales
	
	//Herbivoros
	private EspecieAnimal herbivoro1;
	private EspecieAnimal herbivoro2;
	private EspecieAnimal herbivoro3;
	private EspecieAnimal herbivoro4;
	private EspecieAnimal herbivoro5;
	//Carnivoros
	private EspecieAnimal carnivoro1;
	private EspecieAnimal carnivoro2;
	private EspecieAnimal carnivoro3;
	private EspecieAnimal carnivoro4;
	private EspecieAnimal carnivoro5;
	
	//Tipos de plantas
	private EspecieVegetal seta;
	private EspecieVegetal flor;
	private EspecieVegetal palo;
	private EspecieVegetal arbusto;
	private EspecieVegetal estrom;
	private EspecieVegetal cactus;
	private EspecieVegetal palmera;
	private EspecieVegetal pino;
	private EspecieVegetal cipres;
	private EspecieVegetal pinoAlto;
	
	//Descripciones
	private List<string> descripciones;
	private string descripcionSeta				= "Del reino de los hongos, las setas son organismos simples que crecen en llanuras y colinas. No producen mucha comida, pero su reproduccion por esporas es muy efectiva y les permite extenderse a gran distancia.";
	private string descripcionFlor				= "Las flores son plantas peque\u00f1as que crecen en las llanuras, cubriendolas de color. Producen mas comida que las setas, aunque se reproducen menos y de forma mas local.";
	private string descripcionCana				= "Alargadas y finas, las ca\u00f1as crecen en llanuras, costas y desiertos, adaptandose muy bien al entorno en el que se encuentren. No producen mucho alimento, pero pueden expandirse a zonas lejanas.";
	private string descripcionArbusto			= "Los arbustos son plantas muy resistentes, que crecen en llanuras y colinas. Lo mas destacable de ellas es su adaptabilidad, pues una vez que se afianzan en un lugar se adaptan muy bien a su entorno.";
	private string descripcionEstromatolito		= "A medio camino entre las algas y el coral, los estromatolitos forman sus estructuras en las costas. Son organismos muy locales y de reproduccion lenta, pero muy estables y resistentes.";
	private string descripcionCactus			= "Los cactus se extienden por los desiertos y los terrenos volcanicos. Son una fuente muy rica en alimentos para las especies que se alimentan de ellos y pueden reproducirse a grandes distancias.";
	private string descripcionPalmera			= "El habitat natural de la palmera es la costa, aunque tambien pueden encontrarse en llanuras. Son plantas muy resistentes, con una alta produccion de alimento y una gran capacidad de adaptacion al medio.";
	private string descripcionPino				= "Los pinos son arboles de gran tama\u00f1o que crecen en llanuras, colinas y en menor medida en monta\u00f1as. Ademas de su gran resistencia, se adaptan muy bien al entorno en el que crecen.";
	private string descripcionCipres			= "Pocos habitats existen donde un cipres no pueda nacer. Estos arboles crecen muy rapido y tienen un factor de adaptacion extraordinariamente alto, lo que les permite sobrevivir en todo tipo de lugares.";
	private string descripcionPinoAlto			= "El pino alto se encuentra con frecuencia en colinas y monta\u00f1as, aunque puede vivir en mas lugares. Es un arbol enorme y con muchas hojas que representa lo mas alto en la cadena de los vegetales.";
	private string descripcionHerb1				= "RELLENAR";
	private string descripcionHerb2				= "RELLENAR";
	private string descripcionHerb3				= "RELLENAR";
	private string descripcionHerb4				= "RELLENAR";
	private string descripcionHerb5				= "RELLENAR";
	private string descripcionCarn1				= "RELLENAR";
	private string descripcionCarn2				= "RELLENAR";
	private string descripcionCarn3				= "RELLENAR";
	private string descripcionCarn4				= "RELLENAR";
	private string descripcionCarn5				= "RELLENAR";
	private string descripcionFabBas			= "En este edificio se producen componentes de construccion basicos, necesarios para casi todas las construcciones que puede erigir la nave y para las reparaciones.";
	private string descripcionEnergia1			= "A traves de un reactor de fusion y otro de fision, este edificio produce una alta cantidad de energia. Este combinado de reacciones es practicamente inagotable y limpio.";
	private string descripcionGranja			= "Recolectando seres vivos de los alrededores, la granja es capaz de refinar compuestos de material biologico esenciales para el fin de la mision encomendada a la nave.";
	private string descripcionFabAdv			= "La fabrica de componentes avanzados recolecta ciertos metales raros para producir materiales complejos y muy valiosos, permitiendo construir e investigar en nuevos frentes.";
	private string descripcionEnergia2			= "Creando un campo de contencion magnetica muy poderoso, este edificio capta y utiliza materia oscura para producir una ingente cantidad de energia.";
	
	//Variables del script
	private ModelosEdificios modelosEdificios;
	private ModelosVegetales modelosVegetales;
	private ModelosAnimales modelosAnimales;
	private Principal principal;
	
	void Awake() {
		modelosEdificios = GameObject.FindGameObjectWithTag("ModelosEdificios").GetComponent<ModelosEdificios>();		
		modelosVegetales = GameObject.FindGameObjectWithTag("ModelosVegetales").GetComponent<ModelosVegetales>();		
		modelosAnimales = GameObject.FindGameObjectWithTag("ModelosAnimales").GetComponent<ModelosAnimales>();
		principal = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Principal>();
		
		descripciones = new List<string>();
		
		List<T_habitats> habsEdificios = new List<T_habitats>();
		habsEdificios.Add(T_habitats.llanura);
		habsEdificios.Add(T_habitats.colina);
		
		List<T_habitats> habsEdificiosAdv = new List<T_habitats>();
		habsEdificiosAdv.Add(T_habitats.costa);
		habsEdificiosAdv.Add(T_habitats.llanura);
		habsEdificiosAdv.Add(T_habitats.colina);
		habsEdificiosAdv.Add(T_habitats.tundra);
		habsEdificiosAdv.Add(T_habitats.desierto);
		
		/*fabricaComBas = new TipoEdificio("Fábrica de componentes básicos",habsEdificios,100,25,0,0,T_elementos.comunes,modelosEdificios.fabCompBas);
		energia = new TipoEdificio("Central de energía",habsEdificios,150,15,0,0,T_elementos.comunes,modelosEdificios.centralEnergia);
		granja = new TipoEdificio("Granja",habsEdificios,700,200,50,0,T_elementos.nada,modelosEdificios.granja);
		fabricaComAdv = new TipoEdificio("Fábrica de componentes avanzados",habsEdificiosAdv,850,300,0,0,T_elementos.raros,modelosEdificios.fabCompAdv);
		energiaAdv = new TipoEdificio("Central de energía avanzada",habsEdificiosAdv,1000,500,250,0,T_elementos.raros,modelosEdificios.centralEnergiaAdv);
		*/
		fabricaComBas = new TipoEdificio("Fábrica de componentes básicos",habsEdificios,0,0,0,0,T_elementos.comunes,10,0,0,0,0,100,0,0,modelosEdificios.fabCompBas);
		energia = new TipoEdificio("Central de energía",habsEdificios,0,0,0,0,T_elementos.comunes,0,0,0,0,200,0,0,0,modelosEdificios.centralEnergia);
		granja = new TipoEdificio("Granja",habsEdificios,0,0,0,0,T_elementos.nada,1000,0,0,0,0,0,0,0,modelosEdificios.granja);
		fabricaComAdv = new TipoEdificio("Fábrica de componentes avanzados",habsEdificiosAdv,0,0,0,0,T_elementos.raros,1000,0,0,0,0,0,500,0,modelosEdificios.fabCompAdv);
		energiaAdv = new TipoEdificio("Central de energía avanzada",habsEdificiosAdv,0,0,0,0,T_elementos.raros,0,0,0,0,1000,0,0,0,modelosEdificios.centralEnergiaAdv);
		
		/* Vegetales */
		/*vegetal = new EspecieVegetal(nombre, numMaxSeresEspecie, numMaxVegetales, numIniVegetales, capacidadMigracionLocal, capacidadMigracionGlobal, radioMigracion, 
									   turnosEvolucionInicial, evolucion, habitabilidadInicial, idTextura, modelos
		nombre						=>	nombre de la especie
		numMaxSeresEspecie			=>	número máximo de seres de esa especie
		numMaxVegetales				=>	número máximo de vegetales en una misma casilla. Valores desde 1000 para la primera especie y aumentando en sucesivas especies.
		numIniVegetales				=>	número inicial de vegetales al crearse. Valores desde 100 para la primera especie y aumentando en sucesivas especies.
		capacidadMigracionLocal		=>	probabilidad en tanto por 1 de que una especie migre a una casilla colindante. La migración también depende de la habitabilidad y del numVegetales. 
										Así que si por ejemplo tenemos 0.5f en migracionLocal y 0.5 en la habitabilidad de ese hábitat hay un 25% de que migre localmente en el caso de que la
										planta este al 100% (numVegetales = numMaxVegetales). Si por ejemplo estuviera a 100/1000 (caso inicial) pues tendría un 2.5% de probabilidad de migrar.
										Cuando la planta este al 100% (en 10 turnos) pues migrara con un 25% de probabilidad
		capacidadMigracionGlobal	=>	igual que la local sólo que prueba a migrar a una casilla que puede estar entre 1 y radioMigración de distancia en cualquier direccion
		radioMigracion				=>	distancia a la que puede migrar globalmente un vegetal
		turnosEvolucionInicial		=>	turnos que tienen que pasar para que un vegetal mejore la habitabilidad del hábitat en el que está
		evolucion					=>	mejora que se produce cuando turnosEvolucion llega a 0
		habitabilidadInicial		=>	lista de floats desde -1.0f (inhabitable) a 0.0 (decreciente) y hasta 1.0f (creciente). La lista tiene que ser del mismo tamaño que el total de los habitats
		idTextura					=>	id de la textura que se pinta debajo del modelo
		modelos						=>	lista de los diferentes modelos que tiene la especie		
		*/
		//Seta: habitats -> llanura y colina
		//Tier 1. Barata y normal en llanura y colina. Poca produccion de comida pero alta reproductibilidad y rango de migracion
		List<float> habSeta = new List<float>();
		habSeta.Add(-0.1f);//montana
		habSeta.Add( 0.2f);//llanura
		habSeta.Add( 0.3f);//colina
		habSeta.Add(-1.0f);//desierto
		habSeta.Add(-1.0f);//volcanico
		habSeta.Add(-1.0f);//mar
		habSeta.Add(-0.6f);//costa
		habSeta.Add(-1.0f);//tundra
		habSeta.Add(-1.0f);//inhabitable		
		seta = new EspecieVegetal("Seta",50,1000,100,0.05f,0.02f,7,20,0.01f,habSeta,1,modelosVegetales.setas);
		
		//Flor: habitats -> llanura
		//Tier 1. Barata y decente en llanuras. Mediocre produccion de alimento, reproductibilidad buena pero rango de migracion normal.
		List<float> habFlor = new List<float>();
		habFlor.Add(-0.4f);//montana
		habFlor.Add( 0.5f);//llanura
		habFlor.Add(-0.3f);//colina
		habFlor.Add(-1.0f);//desierto
		habFlor.Add(-1.0f);//volcanico
		habFlor.Add(-1.0f);//mar
		habFlor.Add(-1.0f);//costa
		habFlor.Add(-1.0f);//tundra
		habFlor.Add(-1.0f);//inhabitable		
		flor = new EspecieVegetal("Flor",50,1200,150,0.05f,0.02f,4,10,0.01f,habFlor,2,modelosVegetales.flores);//0.3f,0.2f,4,10,0.01f,habFlor,2,modelosVegetales.flores);

		//Palo (Caña): habitats -> llanura, costa y desierto
		//Tier 2. Normal en llanura, desierto y costa, produccion de alimento normal, reproductibilidad normal y rango alto. Alto ratio de evolucion.
		List<float> habCana = new List<float>();
		habCana.Add(-1.0f);//montana
		habCana.Add( 0.3f);//llanura
		habCana.Add(-0.5f);//colina
		habCana.Add( 0.2f);//desierto
		habCana.Add(-1.0f);//volcanico
		habCana.Add(-1.0f);//mar
		habCana.Add( 0.3f);//costa
		habCana.Add(-1.0f);//tundra
		habCana.Add(-1.0f);//inhabitable		
		palo = new EspecieVegetal("Caña",50,1500,200,0.05f,0.01f,6,8,0.04f,habCana,3,modelosVegetales.canas);
		
		//Arbusto: habitats -> llanura, colina, montaña y desierto
		//Tier 2. Decente en llanura y colina. Produccion normal y reproductibilidad normal. Rango bajo y evolucion lenta pero alta.
		List<float> habArbusto = new List<float>();
		habArbusto.Add(-0.3f);//montana
		habArbusto.Add( 0.3f);//llanura
		habArbusto.Add( 0.4f);//colina
		habArbusto.Add(-0.2f);//desierto
		habArbusto.Add(-1.0f);//volcanico
		habArbusto.Add(-1.0f);//mar
		habArbusto.Add(-1.0f);//costa
		habArbusto.Add(-1.0f);//tundra
		habArbusto.Add(-1.0f);//inhabitable		
		arbusto = new EspecieVegetal("Arbusto",50,1800,120,0.04f,0.01f,3,20,0.1f,habArbusto,2,modelosVegetales.arbustos);

		//Estrom (Estromatolito): habitats -> costa, desierto y volcanico
		//Tier 3. Muy buena en costas. Alta produccion de alimento y reproductibilidad un poco baja. Poca migracion y poco radio. Evolucion decente.
		List<float> habEstrom = new List<float>();
		habEstrom.Add(-1.0f);//montana
		habEstrom.Add(-1.0f);//llanura
		habEstrom.Add(-1.0f);//colina
		habEstrom.Add(-0.8f);//desierto
		habEstrom.Add(-0.1f);//volcanico
		habEstrom.Add(-1.0f);//mar
		habEstrom.Add( 0.8f);//costa
		habEstrom.Add(-0.4f);//tundra
		habEstrom.Add(-1.0f);//inhabitable		
		estrom = new EspecieVegetal("Estromatolito",50,2400,350,0.03f,0.01f,3,25,0.03f,habEstrom,4,modelosVegetales.estromatolitos);

		//Cactus: habitats -> desierto
		//Tier 3. Muy buena en desierto y buena en volcanico. Produccion de alimento decente, reproductibilidad normal, alto ratio de migracion y alta evolucion.
		List<float> habCactus = new List<float>();
		habCactus.Add(-0.8f);//montana
		habCactus.Add(-0.6f);//llanura
		habCactus.Add(-1.0f);//colina
		habCactus.Add( 0.7f);//desierto
		habCactus.Add( 0.5f);//volcanico
		habCactus.Add(-1.0f);//mar
		habCactus.Add(-1.0f);//costa
		habCactus.Add(-1.0f);//tundra
		habCactus.Add(-1.0f);//inhabitable		
		cactus = new EspecieVegetal("Cactus",50,2200,250,0.03f,0.03f,8,12,0.07f,habCactus,0,modelosVegetales.cactus);

		//Palmera: habitats -> costa
		//Tier 4. Buenisima en costas y mala en llanuras. Alta produccion, reproductibilidad buena, alta migracion y alta evolucion.
		List<float> habPalm = new List<float>();
		habPalm.Add(-1.0f);//montana
		habPalm.Add( 0.2f);//llanura
		habPalm.Add(-1.0f);//colina
		habPalm.Add(-0.2f);//desierto
		habPalm.Add(-1.0f);//volcanico
		habPalm.Add(-1.0f);//mar
		habPalm.Add( 0.9f);//costa
		habPalm.Add(-1.0f);//tundra
		habPalm.Add(-1.0f);//inhabitable		
		palmera = new EspecieVegetal("Palmera",50,3000,400,0.04f,0.03f,5,15,0.1f,habPalm,3,modelosVegetales.palmeras);

		//Pino: habitats -> tundra, colina y montaña
		//Tier 4. Muy buena en colinas, buena en llanura y mediocreo en montañas. Produccion buena, reproductibilidad buena, migracion mediocre y evolucion alta.
		List<float> habPino = new List<float>();
		habPino.Add( 0.3f);//montana
		habPino.Add( 0.5f);//llanura
		habPino.Add( 0.7f);//colina
		habPino.Add(-1.0f);//desierto
		habPino.Add(-1.0f);//volcanico
		habPino.Add(-1.0f);//mar
		habPino.Add(-0.1f);//costa
		habPino.Add(-1.0f);//tundra
		habPino.Add(-1.0f);//inhabitable		
		pino = new EspecieVegetal("Pino",50,3500,250,0.05f,0.02f,5,20,0.15f,habPino,4,modelosVegetales.pinos);

		//Ciprés: habitats -> tundra, colina y montaña
		//Tier 5. Bueno en muchos habitats. Procuddion muy alta, reproduccion buena, migracion normal y evolucion altisima.
		List<float> habCipres = new List<float>();
		habCipres.Add( 0.6f);//montana
		habCipres.Add( 0.2f);//llanura
		habCipres.Add( 0.4f);//colina
		habCipres.Add(-0.8f);//desierto
		habCipres.Add(-0.5f);//volcanico
		habCipres.Add(-1.0f);//mar
		habCipres.Add(-0.1f);//costa
		habCipres.Add( 0.6f);//tundra
		habCipres.Add(-1.0f);//inhabitable		
		cipres = new EspecieVegetal("Ciprés",50,4000,450,0.06f,0.02f,6,10,0.2f,habCipres,4,modelosVegetales.cipreses);

		//Pino Alto: habitats -> tundra y montaña
		//Tier 5. Muy bueno en montaña y colina, y decente en mas. Produccion altisima, reproduccion buena, migracion alta y evolucion muy alta.
		List<float> habPinoAlto = new List<float>();
		habPinoAlto.Add( 0.9f);//montana
		habPinoAlto.Add( 0.5f);//llanura
		habPinoAlto.Add( 0.9f);//colina
		habPinoAlto.Add(-1.0f);//desierto
		habPinoAlto.Add(-1.0f);//volcanico
		habPinoAlto.Add(-1.0f);//mar
		habPinoAlto.Add(-0.5f);//costa
		habPinoAlto.Add( 0.3f);//tundra
		habPinoAlto.Add(-1.0f);//inhabitable		
		pinoAlto = new EspecieVegetal("Pino Alto",50,5000,600,0.05f,0.02f,6,12,0.2f,habPinoAlto,1,modelosVegetales.pinosAltos);
		
		
		List<T_habitats> listaHabs = new List<T_habitats>();
		listaHabs.Add(T_habitats.costa);
		listaHabs.Add(T_habitats.llanura);
		listaHabs.Add(T_habitats.colina);
		listaHabs.Add(T_habitats.tundra);
		listaHabs.Add(T_habitats.desierto);
		
		/* Herbivoros */
		listaHabs.Clear();
		listaHabs.Add(T_habitats.llanura);
		listaHabs.Add(T_habitats.colina);
		listaHabs.Add(T_habitats.costa);
		
		//Conejo: habitats -> costa, llanura y colina		
		List<Animation> animacionesHerb1 = new List<Animation>();
		/*animacionesHerb1.Add(nacer);
		animacionesHerb1.Add(descansar);
		animacionesHerb1.Add(buscarAlimento);
		animacionesHerb1.Add(comer);
		animacionesHerb1.Add(morir);*/
		herbivoro1 = new EspecieAnimal("Conejo",10,75,2000,500,4,5,tipoAlimentacionAnimal.herbivoro,listaHabs,modelosAnimales.herbivoro1,animacionesHerb1);
		
		listaHabs.Add(T_habitats.desierto);
		//Cabra: habitats -> llanura, colina y desierto
		List<Animation> animacionesHerb2 = new List<Animation>();
		/*animacionesHerb2.Add(nacer);
		animacionesHerb2.Add(descansar);
		animacionesHerb2.Add(buscarAlimento);
		animacionesHerb2.Add(comer);
		animacionesHerb2.Add(morir);*/
		herbivoro2 = new EspecieAnimal("Camello",10,100,1000,250,5,5,tipoAlimentacionAnimal.herbivoro,listaHabs,modelosAnimales.herbivoro2,animacionesHerb2);
		
		listaHabs.Clear();
		listaHabs.Add(T_habitats.costa);
		//Tortuga: habitats -> costa
		List<Animation> animacionesHerb3 = new List<Animation>();
		/*animacionesHerb3.Add(nacer);
		animacionesHerb3.Add(descansar);
		animacionesHerb3.Add(buscarAlimento);
		animacionesHerb3.Add(comer);
		animacionesHerb3.Add(morir);*/
		herbivoro3 = new EspecieAnimal("Tortuga",10,100,1000,250,5,5,tipoAlimentacionAnimal.herbivoro,listaHabs,modelosAnimales.herbivoro3,animacionesHerb3);
		
		listaHabs.Clear();
		listaHabs.Add(T_habitats.colina);
		listaHabs.Add(T_habitats.montana);
		listaHabs.Add(T_habitats.tundra);
		//Ciervo: habitats -> colina, tundra y montaña
		List<Animation> animacionesHerb4 = new List<Animation>();
		/*animacionesHerb4.Add(nacer);
		animacionesHerb4.Add(descansar);
		animacionesHerb4.Add(buscarAlimento);
		animacionesHerb4.Add(comer);
		animacionesHerb4.Add(morir);*/
		herbivoro4 = new EspecieAnimal("Ciervo",10,100,1000,250,5,5,tipoAlimentacionAnimal.herbivoro,listaHabs,modelosAnimales.herbivoro4,animacionesHerb4);
		
		listaHabs.Remove(T_habitats.tundra);
		listaHabs.Add(T_habitats.volcanico);
		//Salamandra: habitats-> colina, montaña y volcanico
		List<Animation> animacionesHerb5 = new List<Animation>();
		/*animacionesHerb5.Add(nacer);
		animacionesHerb5.Add(descansar);
		animacionesHerb5.Add(buscarAlimento);
		animacionesHerb5.Add(comer);
		animacionesHerb5.Add(morir);*/
		herbivoro5 = new EspecieAnimal("Salamandra",10,100,1000,250,5,5,tipoAlimentacionAnimal.herbivoro,listaHabs,modelosAnimales.herbivoro5,animacionesHerb5);
		
		/* Carnivoros */
		listaHabs.Clear();
		listaHabs.Add(T_habitats.llanura);
		listaHabs.Add(T_habitats.colina);
		listaHabs.Add(T_habitats.costa);
		//Zorro: habitats -> costa, llanura y colina
		List<Animation> animacionesCarn1 = new List<Animation>();
		/*animacionesCarn1.Add(nacer);
		animacionesCarn1.Add(descansar);
		animacionesCarn1.Add(buscarAlimento);
		animacionesCarn1.Add(comer);
		animacionesCarn1.Add(morir);*/
		carnivoro1 = new EspecieAnimal("Zorro",10,100,1000,250,5,5,tipoAlimentacionAnimal.carnivoro,listaHabs,modelosAnimales.carnivoro1,animacionesCarn1);
		
		listaHabs.Add(T_habitats.tundra);
		listaHabs.Add(T_habitats.montana);
		listaHabs.Remove(T_habitats.costa);
		//Lobo: habitats -> llanura, colina, tundra y montaña
		List<Animation> animacionesCarn2 = new List<Animation>();
		/*animacionesCarn2.Add(nacer);
		animacionesCarn2.Add(descansar);
		animacionesCarn2.Add(buscarAlimento);
		animacionesCarn2.Add(comer);
		animacionesCarn2.Add(morir);*/
		carnivoro2 = new EspecieAnimal("Lobo",10,100,1000,250,5,5,tipoAlimentacionAnimal.carnivoro,listaHabs,modelosAnimales.carnivoro2,animacionesCarn2);
		
		listaHabs.Add(T_habitats.desierto);
		listaHabs.Remove(T_habitats.montana);
		listaHabs.Remove(T_habitats.tundra);
		//Serpiente: habitats -> llanura, desierto y colina
		List<Animation> animacionesCarn3 = new List<Animation>();
		/*animacionesCarn3.Add(nacer);
		animacionesCarn3.Add(descansar);
		animacionesCarn3.Add(buscarAlimento);
		animacionesCarn3.Add(comer);
		animacionesCarn3.Add(morir);*/
		carnivoro3 = new EspecieAnimal("Serpiente",10,100,1000,250,5,5,tipoAlimentacionAnimal.carnivoro,listaHabs,modelosAnimales.carnivoro3,animacionesCarn3);
		
		listaHabs.Add(T_habitats.montana);
		listaHabs.Remove(T_habitats.desierto);
		//Tigre: habitats -> llanura, colina, montaña
		List<Animation> animacionesCarn4 = new List<Animation>();
		/*animacionesCarn4.Add(nacer);
		animacionesCarn4.Add(descansar);
		animacionesCarn4.Add(buscarAlimento);
		animacionesCarn4.Add(comer);
		animacionesCarn4.Add(morir);*/
		carnivoro4 = new EspecieAnimal("Tigre",10,50,1500,750,5,5,tipoAlimentacionAnimal.carnivoro,listaHabs,modelosAnimales.carnivoro4,animacionesCarn4);
		
		listaHabs.Add(T_habitats.tundra);
		listaHabs.Add(T_habitats.volcanico);
		listaHabs.Remove(T_habitats.montana);
		//Velocitaptor: habitats -> llanura, colina, tundra y volcanico
		List<Animation> animacionesCarn5 = new List<Animation>();
		/*animacionesCarn5.Add(nacer);
		animacionesCarn5.Add(descansar);
		animacionesCarn5.Add(buscarAlimento);
		animacionesCarn5.Add(comer);
		animacionesCarn5.Add(morir);*/
		carnivoro5 = new EspecieAnimal("Velociraptor",10,100,1000,250,5,5,tipoAlimentacionAnimal.carnivoro,listaHabs,modelosAnimales.carnivoro5,animacionesCarn5);
		
	}
	
	void Start () {		
		//NO CAMBIAR EL ORDEN CON EL QUE SE AÑADEN
		//Añadir las descripciones
		descripciones.Add(descripcionSeta);
		descripciones.Add(descripcionFlor);
		descripciones.Add(descripcionCana);
		descripciones.Add(descripcionArbusto);
		descripciones.Add(descripcionEstromatolito);
		descripciones.Add(descripcionCactus);
		descripciones.Add(descripcionPalmera);
		descripciones.Add(descripcionPino);
		descripciones.Add(descripcionCipres);
		descripciones.Add(descripcionPinoAlto);
		descripciones.Add(descripcionHerb1);
		descripciones.Add(descripcionHerb2);
		descripciones.Add(descripcionHerb3);
		descripciones.Add(descripcionHerb4);
		descripciones.Add(descripcionHerb5);
		descripciones.Add(descripcionCarn1);
		descripciones.Add(descripcionCarn2);
		descripciones.Add(descripcionCarn3);
		descripciones.Add(descripcionCarn4);
		descripciones.Add(descripcionCarn5);
		descripciones.Add(descripcionFabBas);
		descripciones.Add(descripcionEnergia1);
		descripciones.Add(descripcionGranja);
		descripciones.Add(descripcionFabAdv);
		descripciones.Add(descripcionEnergia2);
		
		//Añadir a Vida los edificios
		principal.anadeTipoEdificio(fabricaComBas);
		principal.anadeTipoEdificio(energia);
		principal.anadeTipoEdificio(granja);
		principal.anadeTipoEdificio(fabricaComAdv);
		principal.anadeTipoEdificio(energiaAdv);
		
		//Añadir a Vida los vegetales
		principal.anadeEspecieVegetal(seta);
		principal.anadeEspecieVegetal(flor);
		principal.anadeEspecieVegetal(palo);
		principal.anadeEspecieVegetal(arbusto);
		principal.anadeEspecieVegetal(estrom);
		principal.anadeEspecieVegetal(cactus);
		principal.anadeEspecieVegetal(palmera);
		principal.anadeEspecieVegetal(pino);
		principal.anadeEspecieVegetal(cipres);
		principal.anadeEspecieVegetal(pinoAlto);
		
		//Añadir a Vida los herbivoros
		principal.anadeEspecieAnimal(herbivoro1);
		principal.anadeEspecieAnimal(herbivoro2);
		principal.anadeEspecieAnimal(herbivoro3);
		principal.anadeEspecieAnimal(herbivoro4);
		principal.anadeEspecieAnimal(herbivoro5);
		
		//Añadir a Vida los carnivoros
		principal.anadeEspecieAnimal(carnivoro1);
		principal.anadeEspecieAnimal(carnivoro2);
		principal.anadeEspecieAnimal(carnivoro3);
		principal.anadeEspecieAnimal(carnivoro4);
		principal.anadeEspecieAnimal(carnivoro5);
		
		//principal.vida.actualizaNumTurnos();
	}
	
	public string getDescripcion(int entrada) {
		if (entrada >= 0 && entrada < descripciones.Count)
			return descripciones[entrada];
		return "";
	}
	
	public int getNumeroSer(EspecieAnimal entrada) {
		return entrada.idEspecie + 10;
	}
	
	public int getNumeroSer(EspecieVegetal entrada) {
		return entrada.idEspecie;
	}
	
	public int getNumeroSer(TipoEdificio entrada) {
		return entrada.idTipoEdificio + 20;
	}
}
