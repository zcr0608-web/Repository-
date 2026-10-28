using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MobileStrikePrototypeBootstrap : MonoBehaviour
{
    [Header("Prototype bootstrap")]
    public bool buildSceneOnPlay = true;
    private GameObject menuPanel;
    private GameObject hudPanel;
    private Text healthText;
    private Text ammoText;
    private Text messageText;
    private MobileStrikeWeapon weapon;
    private MobileStrikePlayerHealth health;
    private MobileStrikePlayerController playerController;
    private bool gameStarted;

    private void Start()
    {
        if (buildSceneOnPlay) BuildPrototype();
    }

    private void BuildPrototype()
    {
        CreateLightingAndMap();
        CreatePlayer();
        CreateTargets();
        CreateUI();
        Time.timeScale = 0f;
        ShowMenu(true);
    }

    private void CreateLightingAndMap()
    {
        RenderSettings.ambientLight = new Color(0.55f, 0.59f, 0.65f);
        GameObject lightObj = new GameObject("Sun Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        lightObj.transform.rotation = Quaternion.Euler(48f, -32f, 0f);

        MakeCube("Ground", new Vector3(0f, -0.5f, 0f), new Vector3(50f, 1f, 50f), new Color(0.19f, 0.25f, 0.22f));
        MakeCube("North Wall", new Vector3(0f, 1.5f, 22f), new Vector3(44f, 3f, 1f), new Color(0.25f, 0.29f, 0.31f));
        MakeCube("South Wall", new Vector3(0f, 1.5f, -22f), new Vector3(44f, 3f, 1f), new Color(0.25f, 0.29f, 0.31f));
        MakeCube("East Wall", new Vector3(22f, 1.5f, 0f), new Vector3(1f, 3f, 44f), new Color(0.25f, 0.29f, 0.31f));
        MakeCube("West Wall", new Vector3(-22f, 1.5f, 0f), new Vector3(1f, 3f, 44f), new Color(0.25f, 0.29f, 0.31f));
        for (int i = 0; i < 12; i++)
        {
            float x = (i % 4) * 8f - 12f;
            float z = (i / 4) * 10f - 8f;
            MakeCube("Concrete Cover " + i, new Vector3(x, 0.65f, z), new Vector3(2.4f, 1.3f, 2.4f), new Color(0.31f, 0.34f, 0.33f));
        }
        // Training lane marker strips
        for (int i = 0; i < 5; i++)
            MakeCube("Lane Marker " + i, new Vector3(-15f + i * 7.5f, 0.015f, 13f), new Vector3(3f, 0.03f, 0.18f), new Color(0.8f, 0.68f, 0.25f));
    }

    private GameObject MakeCube(string objectName, Vector3 position, Vector3 scale, Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = objectName;
        go.transform.position = position;
        go.transform.localScale = scale;
        Renderer renderer = go.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = color;
        return go;
    }

    private void CreatePlayer()
    {
        GameObject player = new GameObject("Player");
        player.transform.position = new Vector3(0f, 0.1f, -15f);
        CharacterController cc = player.AddComponent<CharacterController>();
        cc.height = 1.8f;
        cc.radius = 0.35f;
        cc.center = new Vector3(0f, 0.9f, 0f);
        playerController = player.AddComponent<MobileStrikePlayerController>();
        health = player.AddComponent<MobileStrikePlayerHealth>();

        GameObject pivot = new GameObject("Camera Pivot");
        pivot.transform.SetParent(player.transform, false);
        pivot.transform.localPosition = new Vector3(0f, 1.55f, 0f);
        playerController.cameraPivot = pivot.transform;

        GameObject cameraObj = new GameObject("Player Camera");
        cameraObj.transform.SetParent(pivot.transform, false);
        cameraObj.transform.localPosition = Vector3.zero;
        Camera cam = cameraObj.AddComponent<Camera>();
        cam.fieldOfView = 75f;
        cam.tag = "MainCamera";
        cameraObj.AddComponent<AudioListener>();

        GameObject gun = MakeCube("First Person Rifle (prototype)", new Vector3(0f, 0f, 0f), new Vector3(0.16f, 0.16f, 0.62f), new Color(0.12f, 0.14f, 0.14f));
        gun.transform.SetParent(cameraObj.transform, false);
        gun.transform.localPosition = new Vector3(0.28f, -0.25f, 0.55f);
        gun.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        weapon = cameraObj.AddComponent<MobileStrikeWeapon>();
        weapon.playerCamera = cam;
        weapon.recoilPivot = pivot.transform;
        weapon.weaponVisuals = new GameObject[] { gun };
        weapon.muzzleFlash = null;
    }

    private void CreateTargets()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject target = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            target.name = "Practice Target " + (i + 1);
            target.transform.position = new Vector3(-12f + i * 6f, 1f, 8f + (i % 2) * 3f);
            target.transform.localScale = new Vector3(0.8f, 1f, 0.8f);
            target.GetComponent<Renderer>().material.color = new Color(0.55f, 0.19f, 0.16f);
            target.AddComponent<MobileStrikePlayerHealth>();
        }
    }

    private void CreateUI()
    {
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();

        GameObject canvasObj = new GameObject("MobileStrike Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        hudPanel = new GameObject("HUD");
        hudPanel.transform.SetParent(canvasObj.transform, false);
        RectTransform hudRect = hudPanel.AddComponent<RectTransform>();
        hudRect.anchorMin = Vector2.zero; hudRect.anchorMax = Vector2.one;
        hudRect.offsetMin = Vector2.zero; hudRect.offsetMax = Vector2.zero;

        healthText = MakeText(hudPanel.transform, "HEALTH 100", new Vector2(0.02f, 0.91f), new Vector2(0.28f, 0.98f), 30, TextAnchor.MiddleLeft);
        ammoText = MakeText(hudPanel.transform, "ASSAULT RIFLE 30 / 30", new Vector2(0.68f, 0.91f), new Vector2(0.98f, 0.98f), 28, TextAnchor.MiddleRight);
        messageText = MakeText(hudPanel.transform, "TRAINING RANGE", new Vector2(0.35f, 0.91f), new Vector2(0.65f, 0.98f), 24, TextAnchor.MiddleCenter);

        MakeCrosshair(hudPanel.transform);
        CreatePad(hudPanel.transform, "MOVE", new Vector2(0.02f, 0.04f), new Vector2(0.22f, 0.32f), MobileStrikeTouchPad.PadMode.Move);
        CreatePad(hudPanel.transform, "LOOK / DRAG", new Vector2(0.48f, 0.08f), new Vector2(0.97f, 0.88f), MobileStrikeTouchPad.PadMode.Look);
        CreateButton(hudPanel.transform, "FIRE", new Vector2(0.82f, 0.12f), new Vector2(0.96f, 0.28f), () => weapon.ShootOnce(), new Color(0.72f, 0.2f, 0.17f));
        CreateButton(hudPanel.transform, "RELOAD", new Vector2(0.82f, 0.31f), new Vector2(0.96f, 0.39f), () => weapon.Reload(), new Color(0.18f, 0.29f, 0.38f));
        CreateButton(hudPanel.transform, "SWITCH", new Vector2(0.82f, 0.42f), new Vector2(0.96f, 0.50f), () => weapon.SwitchWeapon(), new Color(0.18f, 0.29f, 0.38f));

        menuPanel = new GameObject("Main Menu");
        menuPanel.transform.SetParent(canvasObj.transform, false);
        RectTransform menuRect = menuPanel.AddComponent<RectTransform>();
        menuRect.anchorMin = Vector2.zero; menuRect.anchorMax = Vector2.one;
        menuRect.offsetMin = Vector2.zero; menuRect.offsetMax = Vector2.zero;
        Image shade = menuPanel.AddComponent<Image>();
        shade.color = new Color(0.025f, 0.045f, 0.065f, 0.94f);
        MakeText(menuPanel.transform, "MOBILE STRIKE", new Vector2(0.15f, 0.58f), new Vector2(0.85f, 0.76f), 68, TextAnchor.MiddleCenter).color = new Color(0.3f, 0.9f, 0.84f);
        MakeText(menuPanel.transform, "TAP SHOOTER  •  MODERN COMBAT TRAINING", new Vector2(0.15f, 0.49f), new Vector2(0.85f, 0.59f), 27, TextAnchor.MiddleCenter);
        MakeText(menuPanel.transform, "Prototype build · Practice range · Mobile touch controls", new Vector2(0.15f, 0.43f), new Vector2(0.85f, 0.49f), 20, TextAnchor.MiddleCenter);
        CreateButton(menuPanel.transform, "START TRAINING", new Vector2(0.35f, 0.29f), new Vector2(0.65f, 0.40f), StartGame, new Color(0.08f, 0.48f, 0.48f));
        CreateButton(menuPanel.transform, "QUIT", new Vector2(0.42f, 0.19f), new Vector2(0.58f, 0.27f), QuitGame, new Color(0.25f, 0.28f, 0.31f));

        weapon.onAmmoChanged.AddListener((current, max, name) => { if (ammoText != null) ammoText.text = name.ToUpper() + "  " + current + " / " + max; });
        health.onHealthChanged.AddListener((current, max) => { if (healthText != null) healthText.text = "HEALTH  " + Mathf.CeilToInt(current); });
        health.onDied.AddListener(() => { if (messageText != null) messageText.text = "ELIMINATED"; });
        hudPanel.SetActive(false);
    }

    private Text MakeText(Transform parent, string value, Vector2 anchorMin, Vector2 anchorMax, int size, TextAnchor alignment)
    {
        GameObject go = new GameObject(value);
        go.transform.SetParent(parent, false);
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin; rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
        Text text = go.AddComponent<Text>();
        text.text = value; text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = size; text.alignment = alignment; text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        return text;
    }

    private void MakeCrosshair(Transform parent)
    {
        MakeCrosshairPart(parent, "Crosshair Top", new Vector2(0.5f, 0.5f), new Vector2(0f, 12f), new Vector2(4f, 15f));
        MakeCrosshairPart(parent, "Crosshair Bottom", new Vector2(0.5f, 0.5f), new Vector2(0f, -12f), new Vector2(4f, 15f));
        MakeCrosshairPart(parent, "Crosshair Left", new Vector2(0.5f, 0.5f), new Vector2(-12f, 0f), new Vector2(15f, 4f));
        MakeCrosshairPart(parent, "Crosshair Right", new Vector2(0.5f, 0.5f), new Vector2(12f, 0f), new Vector2(15f, 4f));
    }

    private void MakeCrosshairPart(Transform parent, string objectName, Vector2 anchor, Vector2 position, Vector2 size)
    {
        GameObject go = new GameObject(objectName);
        go.transform.SetParent(parent, false);
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchor; rect.anchorMax = anchor; rect.anchoredPosition = position; rect.sizeDelta = size;
        Image image = go.AddComponent<Image>();
        image.color = new Color(0.6f, 1f, 0.86f, 0.95f);
        image.raycastTarget = false;
    }

    private void CreatePad(Transform parent, string label, Vector2 anchorMin, Vector2 anchorMax, MobileStrikeTouchPad.PadMode mode)
    {
        GameObject go = new GameObject(label + " Pad");
        go.transform.SetParent(parent, false);
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin; rect.anchorMax = anchorMax; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
        Image image = go.AddComponent<Image>();
        image.color = mode == MobileStrikeTouchPad.PadMode.Move ? new Color(0.2f, 0.8f, 0.75f, 0.13f) : new Color(0.2f, 0.8f, 0.75f, 0.035f);
        image.raycastTarget = true;
        MobileStrikeTouchPad pad = go.AddComponent<MobileStrikeTouchPad>();
        pad.player = playerController; pad.mode = mode; pad.padArea = rect;
        if (mode == MobileStrikeTouchPad.PadMode.Move)
        {
            Text t = MakeText(go.transform, "MOVE", new Vector2(0.1f, 0.1f), new Vector2(0.9f, 0.9f), 18, TextAnchor.MiddleCenter);
            t.color = new Color(1f, 1f, 1f, 0.6f);
            t.raycastTarget = false;
        }
    }

    private void CreateButton(Transform parent, string label, Vector2 anchorMin, Vector2 anchorMax, UnityEngine.Events.UnityAction action, Color color)
    {
        GameObject go = new GameObject(label + " Button");
        go.transform.SetParent(parent, false);
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin; rect.anchorMax = anchorMax; rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
        Image image = go.AddComponent<Image>();
        image.color = color;
        Button button = go.AddComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(action);
        Text text = MakeText(go.transform, label, Vector2.zero, Vector2.one, 22, TextAnchor.MiddleCenter);
        text.fontStyle = FontStyle.Bold;
        text.raycastTarget = false;
    }

    private void StartGame()
    {
        gameStarted = true;
        menuPanel.SetActive(false);
        hudPanel.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (messageText != null) messageText.text = "TRAINING RANGE";
    }

    private void ShowMenu(bool show)
    {
        if (menuPanel != null) menuPanel.SetActive(show);
        if (hudPanel != null) hudPanel.SetActive(!show);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Update()
    {
        if (!gameStarted) return;
        if (healthText != null && health != null) healthText.text = "HEALTH  " + Mathf.CeilToInt(health.CurrentHealth);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            ShowMenu(true);
            gameStarted = false;
        }
    }
}
