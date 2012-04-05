using UnityEngine;
using System.Collections;

//using ;

public class Estados : MonoBehaviour {

	//Variables ---------------------------------------------------------------------------------------------------------------------------

	//GUI
	public GUISkin estiloGUI;											//Los estilos diferentes para la GUI, configurables desde el editor
	public GUISkin estiloGUI_Nuevo;
	public GameObject camaraReparaciones;								//Para mostrar las opciones de las reparaciones de la nave
	public GameObject camaraPrincipal;									//Para mostrar el mundo completo (menos escenas especiales)
	private int menuOpcionesInt					= 0;					//Variable de control sobre el menu lateral derecho
	private bool menuAltera						= false;
	private bool menuCamara						= false;
	private bool menuOpcion						= false;
	private int cuantoW							= Screen.width / 48;	//Minima unidad de medida de la interfaz a lo ancho (formato 16/10)
	private int cuantoH							= Screen.height / 30;	//Minima unidad de medida de la interfaz a lo alto (formato 16/10)
	
	//Privadas del script
	private T_estados estado 					= T_estados.principal;	//Los estados por los que pasa el juego
	private Vida vida;													//Tablero lógico del algoritmo
	
	private GameObject contenedorTexturas;								//El contenedor de las texturas de la primera escena
	private float escalaTiempo					= 1.0f;					//La escala temporal a la que se updateará todo
	
	//Opciones
	public GameObject contenedorSonido;									//El objeto que va a contener la fuente del audio
	private AudioSource sonido;											//La fuente del audio
	
	private bool musicaOn 						= true;					//Está la música activada?
	private float musicaVol 					= 0.5f;					//A que volumen?
	private bool sfxOn 							= true;					//Estan los efectos de sonido activados?
	private float sfxVol 						= 0.5f; 				//A que volumen?
	
	//Tooltips
	private Vector3 posicionMouse 				= Vector3.zero;			//Guarda la ultima posicion del mouse		
	private bool activarTooltip 				= false;				//Controla si se muestra o no el tooltip	
	private float ultimoMov 					= 0.0f;					//Ultima vez que se movio el mouse		
	public float tiempoTooltip 					= 0.75f;				//Tiempo que tarda en aparecer el tooltip	
	
	//Menus para guardar
	private Vector2 posicionScroll 				= Vector2.zero;			//La posicion en la que se encuentra la ventana con scroll
	private int numSaves 						= 0;					//El numero de saves diferentes que hay en el directorio respectivo
	private int numSavesExtra 					= 0;					//Numero de saves que hay que no se ven al primer vistazo en la scrollview
	private string[] nombresSaves;										//Los nombres de los ficheros de savegames guardados

	//Tipos especiales ----------------------------------------------------------------------------------------------------------------------
	
	//Añadir los que hagan falta mas tarde
	enum T_estados {inicial, principal, laboratorio, reparaciones, filtros, guardar, opciones, salir, regenerar};
	
	//Funciones auxiliares -----------------------------------------------------------------------------------------------------------------------
	private IEnumerator terremoto(Vector2 coords) {
		Vector2 dir = UnityEngine.Random.insideUnitCircle;
		Vector3 cross1 = new Vector3(dir.x, dir.y, 0);
		Vector3 cross2 = new Vector3(0, 0, 1);
		Vector3 res = Vector3.Cross(cross1, cross2);
		Vector2 dirPerp = new Vector2(res.x, res.y);
		//Hacer un raycast al punto seleccionado o elegir aleatoriamente un punto de la textura para que ocurra ahi el terremoto
		GameObject planeta = GameObject.FindWithTag("Planeta");
		MeshRenderer renderer = planeta.GetComponent<MeshRenderer>();
		Texture2D texturaBase = renderer.sharedMaterial.mainTexture as Texture2D;
		if (coords == Vector2.zero) {
			Debug.LogError("Coords era cero en el metodo terremoto.");
			coords = new Vector2(Random.Range(0, texturaBase.width), Random.Range(0, texturaBase.height));
			Debug.LogError("Coords es ahora: " + coords);
		}
		//TODO Completar esta parte para lanzar las llamadas a FuncTablero.fisura(...)
		//A lo largo de la perpendicular, crear fisuras de forma creciente desde el eje de simetría
		//También puede hacerse paulatinamente respecto al tiempo
		Camera.main.animation.Play("Shake");
		Vector2 desviacion = dirPerp.normalized;
		float longitud = 40.0f;
		float magnitud = -1.0f;
		FuncTablero.fisura(texturaBase, (int)coords.x, (int)coords.y, longitud, magnitud, dir); 
		texturaBase.Apply();
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 5; i++) {
			desviacion = dirPerp * (i + 1);
			longitud -= 6.0f;
			magnitud -= 0.2f;
			FuncTablero.fisura(texturaBase, (int)coords.x + (int)desviacion.x, (int)coords.y + (int)desviacion.y, longitud, magnitud, dir); 
			FuncTablero.fisura(texturaBase, (int)coords.x - (int)desviacion.x, (int)coords.y - (int)desviacion.y, longitud, magnitud, dir);
			texturaBase.Apply();
			yield return new WaitForSeconds(1);
		}		
	}
	
	private IEnumerator corutinaTerremoto() {
		RaycastHit hit;
		Vector2 pixelUV = Vector2.zero;
		int mask = 1 << 9;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask) ) {
			GameObject planeta = GameObject.FindWithTag("Planeta");
			MeshRenderer renderer = planeta.GetComponent<MeshRenderer>();
			Texture2D texturaBase = renderer.sharedMaterial.mainTexture as Texture2D;
			pixelUV = hit.textureCoord;
			pixelUV.x *= (float)texturaBase.width;
			pixelUV.y *= (float)texturaBase.height;
		}
		yield return StartCoroutine(terremoto(pixelUV));
		
	}
	
	private IEnumerator corutinaAnimalCubo() {
		RaycastHit hit;
		bool pinchado = false;
		while (!pinchado) {
			if (Input.GetMouseButton(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				int mask = 1 << 9;
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) {
					Debug.Log("Rayo lanzado correctamente.");
					FuncTablero.creaMesh(hit, 0);
					pinchado = true;
					continue;
				}
			}
			yield return new WaitForSeconds(0.1f);
		}		
	}
	
	//Funciones principales ----------------------------------------------------------------------------------------------------------------------
	private void creacionInicial() {
		Debug.Log("Creando planeta de cero en creacionInicial");
		//Trabajar con la textura Textura_Planeta y crear el mapa lógico a la vez
		GameObject planeta = GameObject.FindWithTag("Planeta");
		MeshRenderer renderer = planeta.GetComponent<MeshRenderer>();
		Texture2D texturaBase = renderer.sharedMaterial.mainTexture as Texture2D;
		Texture2D texturaNorm = renderer.sharedMaterial.GetTexture("_Normals") as Texture2D;	//Los nombres vienen definidos en el editor, en el material
		
		Color[] pixels = new Color[texturaBase.width * texturaBase.height];
		FuncTablero.inicializa(texturaBase);
		
		pixels = FuncTablero.ruidoTextura();											//Se crea el ruido para la textura base y normales...
		pixels = FuncTablero.suavizaBordeTex(pixels, texturaBase.width / 20);			//Se suaviza el borde lateral...
		pixels = FuncTablero.suavizaPoloTex(pixels, texturaBase.height / 20);			//Se suavizan los polos...
		
		texturaBase.SetPixels(pixels);
		texturaBase.Apply();
	
		texturaNorm.SetPixels(pixels);													//Se aplican los pixeles a la textura normal para duplicarlos
		texturaNorm.SetPixels32(FuncTablero.creaNormalMap(texturaNorm));				//se transforma a NormalMap
		texturaNorm.Apply();
		
		estado = T_estados.principal;
	}
	
	//Update y transiciones de estados -------------------------------------------------------------------------------------------------------
	
	void Awake() {
		contenedorTexturas = GameObject.FindGameObjectWithTag("Carga");
		if (contenedorTexturas == null) {
			creacionInicial();
		}
		else {
			//Trabajar con la textura Textura_Planeta y crear el mapa lógico a la vez
			GameObject planeta = GameObject.FindWithTag("Planeta");
			MeshRenderer renderer = planeta.GetComponent<MeshRenderer>();
			Texture2D texturaBase = renderer.sharedMaterial.mainTexture as Texture2D;
			Texture2D texturaNorm = renderer.sharedMaterial.GetTexture("_Normals") as Texture2D;	//Los nombres vienen definidos en el editor, en el material
			ValoresCarga temp = contenedorTexturas.GetComponent<ValoresCarga>();
			texturaBase = temp.texturaBase;
			texturaNorm = temp.texturaNorm;
			texturaBase.Apply();
			texturaNorm.Apply();
		}
		if (PlayerPrefs.GetInt("MusicaOn") == 1)
			musicaOn = true;
		else
			musicaOn = false;
		musicaVol = PlayerPrefs.GetFloat("MusicaVol");
		if (PlayerPrefs.GetInt("SfxOn") == 1)
			sfxOn = true;
		else
			sfxOn = false;
		sfxVol = PlayerPrefs.GetFloat("SfxVol");
		sonido = contenedorSonido.GetComponent<AudioSource>();
		sonido.mute = !musicaOn;
		sonido.volume = musicaVol;
		//esto para que no salgan warnings molestos
		if (sfxOn) {
			float a = sfxVol;
			sfxVol = a;
		}
		else {
		
		}
		numSaves = SaveLoad.FileCount();
		nombresSaves = new string[numSaves];
		nombresSaves = SaveLoad.getFileNames();
		numSavesExtra = numSaves - 3;
		if (numSavesExtra < 0)
			numSavesExtra = 0;
	}
	
	void Update () {
	
		switch (estado) {
			case T_estados.inicial:
				creacionInicial();
				break;
				
			case T_estados.principal:
				break;
				
			case T_estados.regenerar:
				estado = T_estados.inicial;
				break;
				
			case T_estados.filtros:
				break;
				
			case T_estados.laboratorio:
				break;
				
			case T_estados.reparaciones:
				break;
				
			case T_estados.opciones:
				Time.timeScale = 0;
				break;
				
			case T_estados.guardar:
				Time.timeScale = 0;
				break;
				
			case T_estados.salir:
				Application.LoadLevel("Escena_Inicial");
				break;
				
			default:
				//Error!
				Debug.LogError("Estado del juego desconocido! La variable contiene: " + estado);
				break;
		}
		
		//Control del tooltip
		if (Input.mousePosition != posicionMouse) {
			posicionMouse = Input.mousePosition;
			activarTooltip = false;
			ultimoMov = Time.time;
		}
		else {
			if (Time.time >= ultimoMov + tiempoTooltip) {
				activarTooltip = true;
			}
		}
		//Control del timescale
		if (GUI.changed) {
			Time.timeScale = escalaTiempo;
			Time.fixedDeltaTime = 0.02f * escalaTiempo;
		}	
	}
	
	//Funciones OnGUI---------------------------------------------------------------------------------------------------------------------------
	
	void OnGUI() {
		GUI.skin = estiloGUI;
		switch (estado) {
			case T_estados.inicial:
				GUI.skin = estiloGUI;
				GUI.Box(new Rect(cuantoW * 21, cuantoH * 14, cuantoW * 6, cuantoH * 2), "Re-Generando!");
				break;
			case T_estados.opciones:
				GUI.skin = estiloGUI;
				menuOpciones();
				break;
			case T_estados.principal:
				GUI.skin = estiloGUI_Nuevo;
				menuIzquierdaHex();
//				grupoIzquierda();
//				grupoDerecha();
//				sliderTiempo();
				break;
			case T_estados.guardar:
				GUI.skin = estiloGUI;
				menuGuardar();
				break;
			case T_estados.reparaciones:
				GUI.skin = estiloGUI;
				menuReparaciones();
				break;
			default:						
				break;
		}
		
		//Tooltip
		if (activarTooltip) {
			int longitud = GUI.tooltip.Length;
			if (longitud == 0) {
				return;
			}
			else {
				longitud *= 9;
			}
			float posx = Input.mousePosition.x;
			float posy = Input.mousePosition.y;
			if (posx > (Screen.width / 2)) {
				posx -= 215;
			}
			else {
				posx += 15;
			}
			if (posy > (Screen.height / 2)) {
				posy += 20;
			}		
			Rect pos = new Rect(posx, Screen.height - posy, longitud, 25);
			GUI.Box(pos, "");
			GUI.Label(pos, GUI.tooltip);
		}
		
	}
	
	private void menuIzquierdaHex() {
		barraInformacion(new Rect(cuantoW,cuantoH,cuantoW * 12, cuantoH * 5));
//		GUI.Box(new Rect(cuantoW,cuantoH,cuantoW * 12, cuantoH * 5), new GUIContent(Time.time.ToString(), "Tiempo en el que te encuentras"), "BarraTiempo");
		escalaTiempo = sliderTiempoCompuesto(new Rect(cuantoW,cuantoH * 6, cuantoW * 12, cuantoH * 5), escalaTiempo);
		botonHexCompuestoAltera(new Rect(cuantoW, cuantoH * 11, cuantoW * 12, cuantoH * 5));
		botonHexCompuestoCamara(new Rect(cuantoW, cuantoH * 16, cuantoW * 12, cuantoH * 5));
		botonHexCompuestoOpciones(new Rect(cuantoW, cuantoH * 21, cuantoW * 12, cuantoH * 5));
		GUI.Box(new Rect(cuantoW, cuantoH * 26, cuantoW * 12, cuantoH * 5), "", "BarraAbajo");
	}
	
	/*

	
	private void grupoDerecha() {
		//Dependiendo de que opción este pulsada, poner un menú u otro!
		Control_Raton script;
		Transform objetivo;
		if (menuOpcionesInt == 1) {
			GUI.BeginGroup(new Rect(cuantoW * 43, cuantoH * 10, cuantoW * 5, cuantoH * 12));
			if (GUI.Button(new Rect(0, 0, cuantoW * 5, cuantoH * 4), new GUIContent("", "Click izq. para centrar"), "i_c_fija")) {
				script = transform.parent.GetComponent<Control_Raton>();
				objetivo = GameObject.FindGameObjectWithTag("Planeta").GetComponent<Transform>();
				script.cambiarTarget(objetivo);
				script.cambiarEstado(1);
			}
			if (GUI.Button(new Rect(0, cuantoH * 4, cuantoW * 5, cuantoH * 4), new GUIContent("", "Rotar con click der."), "i_c_rot")) {
				script = transform.parent.GetComponent<Control_Raton>();
				objetivo = GameObject.FindGameObjectWithTag("Planeta").GetComponent<Transform>();
				script.cambiarTarget(objetivo);
				script.cambiarEstado(0);
			}
			if (GUI.Button(new Rect(0, cuantoH * 8, cuantoW * 5, cuantoH * 4), new GUIContent("", "Centrar en la luna"), "i_c_3")) {
				script = transform.parent.GetComponent<Control_Raton>();
				objetivo = GameObject.Find("luna").GetComponent<Transform>();
				script.cambiarTarget(objetivo);
				script.cambiarEstado(0);
			}
			GUI.EndGroup();
		}
		if (menuOpcionesInt == 2) {
			GUI.BeginGroup(new Rect(cuantoW * 43, cuantoH * 10, cuantoW * 5, cuantoH * 12));
			if (GUI.Button(new Rect(0, 0, cuantoW * 5, cuantoH * 4), new GUIContent("", "Laboratorio gen\u00e9tico"), "i_lab")) {
	
			}
			if (GUI.Button(new Rect(0, cuantoH * 4, cuantoW * 5, cuantoH * 4), new GUIContent("", "Visi\u00f3n de la nave"), "i_nav")) {
				camaraPrincipal.GetComponent<Camera>().enabled = false;
				camaraReparaciones.GetComponent<Camera>().enabled = true;
				script = transform.parent.GetComponent<Control_Raton>();
				script.setInteraccion(false);
				estado = T_estados.reparaciones;
			}
			if (GUI.Button(new Rect(0, cuantoH * 8, cuantoW * 5, cuantoH * 4), new GUIContent("", "Opciones del juego"), "i_fil")) {
				script = transform.parent.GetComponent<Control_Raton>();
				script.setInteraccion(false);
				estado = T_estados.opciones;
			}
			GUI.EndGroup();
		}
		if (menuOpcionesInt == 3) {
			GUI.BeginGroup(new Rect(cuantoW * 43, cuantoH * 10, cuantoW * 5, cuantoH * 12));
			if (GUI.Button(new Rect(0, 0, cuantoW * 5, cuantoH * 4), new GUIContent("Fisura", "Crear fisura centrada"), "i_fil")) {
				StartCoroutine(corutinaTerremoto());
			}
			if (GUI.Button(new Rect(0, cuantoH * 4, cuantoW * 5, cuantoH * 4), new GUIContent("Volcan", "Crear volcan centrado"), "i_fil")) {

			}
			if (GUI.Button(new Rect(0, cuantoH * 8, cuantoW * 5, cuantoH * 4), new GUIContent("Animal", "Poner objeto en tablero"), "i_fil")) {
				StartCoroutine(corutinaAnimalCubo());
			}
			GUI.EndGroup();
		}
		
	}
	*/
	private void menuOpciones() {
		Control_Raton script;
		GUILayout.BeginArea(new Rect(cuantoW * 20, cuantoH * 12, cuantoW * 8, cuantoH * 6));
		GUILayout.BeginVertical();
		if (GUILayout.Button(new GUIContent("Salir", "Salir del juego"), "boton_menu_1")) {
			Time.timeScale = 1.0f;
			script = transform.parent.GetComponent<Control_Raton>();
			script.setInteraccion(true);
			estado = T_estados.salir;
		}
		if (GUILayout.Button(new GUIContent("Guardar", "Guardar la partida"), "boton_menu_2")) {
			Time.timeScale = 1.0f;
			nombresSaves = SaveLoad.getFileNames();
			estado = T_estados.guardar;
		}
		if (GUILayout.Button(new GUIContent("Volver", "Volver al juego"), "boton_menu_4")) {
			Time.timeScale = 1.0f;
			script = transform.parent.GetComponent<Control_Raton>();
			script.setInteraccion(true);
			estado = T_estados.principal;
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	private void menuGuardar() {
		Control_Raton script;
		GUI.Box(new Rect(cuantoW * 14, cuantoH * 7, cuantoW * 20, cuantoH * 16), "");
		posicionScroll = GUI.BeginScrollView(new Rect(cuantoW * 14, cuantoH * 8, cuantoW * 20, cuantoH * 14), posicionScroll, new Rect(0, 0, cuantoW * 20, cuantoH * 4 * numSavesExtra));
		if (GUI.Button(new Rect(cuantoW, cuantoH * 4, cuantoW * 18, cuantoH * 4), new GUIContent("Nueva partida salvada", "Guardar una nueva partida"))) {
			ValoresCarga temp = contenedorTexturas.GetComponent<ValoresCarga>();
			string fecha = System.DateTime.Now.ToString().Replace("\\","").Replace("/","").Replace(" ", "").Replace(":","");
			SaveLoad.cambiaFileName("Partida" + fecha + ".hur");
			int tempLong = temp.texturaBase.width * temp.texturaBase.height;
			float[] data = new float[tempLong];
			Color[] pixels = temp.texturaBase.GetPixels();
			for (int i = 0; i < tempLong; i++) {
				data[i] = pixels[i].r;
			}			
			SaveLoad.Save(data,temp.texturaBase.width, temp.texturaBase.height);
			//Recuperar estado normal
			Time.timeScale = 1.0f;
			script = transform.parent.GetComponent<Control_Raton>();
			script.setInteraccion(true);
			estado = T_estados.principal;
		}
		for (int i = 0; i < numSaves; i++) {
			if (GUI.Button(new Rect(cuantoW, (i + 1) * cuantoH * 4, cuantoW * 18, cuantoH * 4), new GUIContent(nombresSaves[i], "Sobreescribir partida num. " + i))) {
				ValoresCarga temp = contenedorTexturas.GetComponent<ValoresCarga>();
				SaveLoad.cambiaFileName(nombresSaves[i]);		
				SaveLoad.Save(temp.texturaBase);
				//Recuperar estado normal
				Time.timeScale = 1.0f;
				script = transform.parent.GetComponent<Control_Raton>();
				script.setInteraccion(true);
				estado = T_estados.principal;
			}
		}
		GUI.EndScrollView();
		if (GUI.Button(new Rect(cuantoW * 42, cuantoH * 26, cuantoW * 4, cuantoH * 2), new GUIContent("Volver", "Volver a la partida"), "boton_atras")) {
			//Recuperar estado normal
			Time.timeScale = 1.0f;
			script = transform.parent.GetComponent<Control_Raton>();
			script.setInteraccion(true);
			estado = T_estados.principal;
		}
		
	}
	
	private void menuReparaciones() {
		if (GUI.Button(new Rect(cuantoW, cuantoH * 20, cuantoW * 2, cuantoH), new GUIContent("Volver", "boton_atras"))) {
			camaraPrincipal.GetComponent<Camera>().enabled = true;
			camaraReparaciones.GetComponent<Camera>().enabled = false;
			Control_Raton script = transform.parent.GetComponent<Control_Raton>();
			script.setInteraccion(true);
			estado = T_estados.principal;
		}
	}
	
//	private void botonHexCompuestoAltera(Rect pos) {
//		GUI.Box(new Rect(pos.x,pos.y,pos.width - cuantoW * 3.9f, pos.height), new GUIContent("", "Opciones para alterar el planeta"), "BarraHexGran");
//		if (menuAltera)
//			GUI.Box(new Rect(pos.x + cuantoW * 8.1f,pos.y,pos.width - cuantoW * 8.1f, pos.height), new GUIContent("", "Opciones (P) para alterar el planeta"), "BarraHexPeq");
//		GUILayout.BeginArea(pos);
//		GUILayout.BeginHorizontal();
//		if (GUILayout.Button("Alterar", "BotonVacio", GUILayout.Width(pos.width / 2))) {
//			menuAltera = true;
//			menuCamara = false;
//			menuOpcion = false;
//			Debug.Log("Pulsado altera grande.");
//		}
//		if (menuAltera) {
//			if (GUILayout.Button(new GUIContent("Peques", "Opciones (P)(B) para alterar tambien"), "BotonVacio", GUILayout.Width(pos.width / 2))) {
//				Debug.Log("Pulsado un peque de altera.");
//			}
//		}
//		GUILayout.EndHorizontal();
//		GUILayout.EndArea();
//		
//	}
	
	private void botonHexCompuestoAltera(Rect pos) {
		GUI.Box(new Rect(pos.x,pos.y,pos.width - cuantoW * 3.9f, pos.height), new GUIContent("", "Opciones para alterar el planeta"), "BarraHexGran");
		if (menuAltera)
			GUI.Box(new Rect(pos.x + cuantoW * 7.0f,pos.y + cuantoH * 0.75f,pos.width - cuantoW * 8.1f, pos.height), new GUIContent("", "Opciones (P) para alterar el planeta"), "BarraHexPeq");
		if (GUI.Button(new Rect(pos.x + cuantoW * 3.5f, pos.y + cuantoH * 0.6f, pos.width - cuantoW * 8.3f, pos.height - cuantoH * 0.9f),"Alterar", "BotonVacio")) {
			menuAltera = true;
			menuCamara = false;
			menuOpcion = false;
			Debug.Log("Pulsado altera grande.");
		}
		if (menuAltera) {
			if (GUI.Button(new Rect(pos.x + cuantoW * 7.7f, pos.y + cuantoH * 0.95f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq1", "Opciones (P)(B) para alterar"), "BotonVacio")) {
				Debug.Log("Pulsado peque altera 1-4");
			}
			if (GUI.Button(new Rect(pos.x + cuantoW * 9.1f, pos.y + cuantoH * 1.85f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq2", "Opciones (P)(B) para alterar"), "BotonVacio")) {
				Debug.Log("Pulsado peque altera 2-4");
			}
			if (GUI.Button(new Rect(pos.x + cuantoW * 7.7f, pos.y + cuantoH * 2.75f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq3", "Opciones (P)(B) para alterar"), "BotonVacio")) {
				Debug.Log("Pulsado peque altera 3-4");
			}
			if (GUI.Button(new Rect(pos.x + cuantoW * 9.1f, pos.y + cuantoH * 3.75f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq4", "Opciones (P)(B) para alterar"), "BotonVacio")) {
				Debug.Log("Pulsado peque altera 4-4");
			}
		}
		
	}
	
	private void botonHexCompuestoCamara(Rect pos) {
		GUI.Box(new Rect(pos.x,pos.y,pos.width - cuantoW * 3.9f, pos.height), new GUIContent("", "Opciones para cambiar la camara"), "BarraHexGran");
		if (menuCamara)
			GUI.Box(new Rect(pos.x + cuantoW * 7.0f,pos.y + cuantoH * 0.75f,pos.width - cuantoW * 8.1f, pos.height), new GUIContent("", "Opciones (P) para cambiar la camara"), "BarraHexPeq");
		if (GUI.Button(new Rect(pos.x + cuantoW * 3.5f, pos.y + cuantoH * 0.6f, pos.width - cuantoW * 8.3f, pos.height - cuantoH * 0.9f), "Camara", "BotonVacio")) {
			menuAltera = false;
			menuCamara = true;
			menuOpcion = false;
			Debug.Log("Pulsado camara grande.");
		}
		if (menuCamara) {
			if (GUI.Button(new Rect(pos.x + cuantoW * 7.7f, pos.y + cuantoH * 0.95f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq1", "Opciones (P)(B) de camara"), "BotonVacio")) {
				Debug.Log("Pulsado peque camara 1-4");
			}
			if (GUI.Button(new Rect(pos.x + cuantoW * 9.1f, pos.y + cuantoH * 1.85f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq2", "Opciones (P)(B) de camara"), "BotonVacio")) {
				Debug.Log("Pulsado peque camara 2-4");
			}
			if (GUI.Button(new Rect(pos.x + cuantoW * 7.7f, pos.y + cuantoH * 2.75f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq3", "Opciones (P)(B) de camara"), "BotonVacio")) {
				Debug.Log("Pulsado peque camara 3-4");
			}
			if (GUI.Button(new Rect(pos.x + cuantoW * 9.1f, pos.y + cuantoH * 3.75f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq4", "Opciones (P)(B) de camara"), "BotonVacio")) {
				Debug.Log("Pulsado peque camara 4-4");
			}
		}
	}
	
	private void botonHexCompuestoOpciones(Rect pos) {
		GUI.Box(new Rect(pos.x,pos.y,pos.width - cuantoW * 3.9f, pos.height), new GUIContent("", "Opciones generales"), "BarraHexGran");
		if (menuOpcion)
			GUI.Box(new Rect(pos.x + cuantoW * 7.0f,pos.y + cuantoH * 0.75f,pos.width - cuantoW * 8.1f, pos.height), new GUIContent("", "Opciones (P) generales"), "BarraHexPeq");
		if (GUI.Button(new Rect(pos.x + cuantoW * 3.5f, pos.y + cuantoH * 0.6f, pos.width - cuantoW * 8.3f, pos.height - cuantoH * 0.9f), "Opciones", "BotonVacio")) {
			menuAltera = false;
			menuCamara = false;
			menuOpcion = true;
			Debug.Log("Pulsado opciones grande.");
		}
		if (menuOpcion) {
			if (GUI.Button(new Rect(pos.x + cuantoW * 7.7f, pos.y + cuantoH * 0.95f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq1", "Opciones (P)(B) generales"), "BotonVacio")) {
				Debug.Log("Pulsado peque opciones 1-4");
			}
			if (GUI.Button(new Rect(pos.x + cuantoW * 9.1f, pos.y + cuantoH * 1.85f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq2", "Opciones (P)(B) generales"), "BotonVacio")) {
				Debug.Log("Pulsado peque opciones 2-4");
			}
			if (GUI.Button(new Rect(pos.x + cuantoW * 7.7f, pos.y + cuantoH * 2.75f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq3", "Opciones (P)(B) generales"), "BotonVacio")) {
				Debug.Log("Pulsado peque opciones 3-4");
			}
			if (GUI.Button(new Rect(pos.x + cuantoW * 9.1f, pos.y + cuantoH * 3.75f, cuantoW * 1.4f, cuantoH * 1.7f), new GUIContent("Peq4", "Opciones (P)(B) generales"), "BotonVacio")) {
				Debug.Log("Pulsado peque opciones 4-4");
			}
		}
	}
	
	private float sliderTiempoCompuesto(Rect pos, float valor) {
		float valorOut;
		GUI.Box(pos, new GUIContent("", "En que tiempo nos encontramos"), "BarraSlider");
		GUILayout.BeginArea(new Rect(pos.x + cuantoW * 3.3f, pos.y + cuantoH * 1.7f, pos.width - cuantoW * 4.3f, pos.height - cuantoH * 2));
		valorOut = GUILayout.HorizontalSlider(valor, 0.2f, 99.9f);
		GUILayout.Label("Escala temporal");
		GUILayout.EndArea();
		return valorOut;
	}
	
	private void barraInformacion(Rect pos) {
		GUI.Box(pos, new GUIContent("", "Tiempo en el que te encuentras"), "BarraTiempo");
		string temp = Time.time.ToString();
		GUI.Label(new Rect(pos.x + cuantoW * 7.0f, pos.y + cuantoH * 2.0f, cuantoW * 3.0f, cuantoH * 2.0f), temp);
	}

}
