using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BoardManager boardManager;
    public LayerMask placeMark;
    public Vector3 placePosition;
    public Col col;
    public Color color;

    [SerializeField] private TestObject beadPrefab;
    [SerializeField] private Camera mainCam;
    [SerializeField] private TMP_Text deleteModeLabel;

    private bool isDeleteMode;
    private Outline selectedColorOutline;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        boardManager.CreatBoard();
        color = GetColorFor(col);
        UpdateDeleteModeLabel();
        CreateColorButtons();
    }

    void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Camera cameraToUse = mainCam != null ? mainCam : Camera.main;
        if (cameraToUse == null)
            return;

        Ray ray = cameraToUse.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            return;

        GameObject clickedObj = hit.collider.gameObject;
        if (isDeleteMode)
        {
            if (!clickedObj.CompareTag("Board"))
                clickedObj.SetActive(false);
            return;
        }

        if (!clickedObj.CompareTag("Board") || beadPrefab == null)
            return;

        placePosition = clickedObj.transform.position + new Vector3(0f, 0.5f, 0f);
        beadPrefab.PlaceBead(placePosition, color);
    }

    public void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode;
        UpdateDeleteModeLabel();
    }

    private void UpdateDeleteModeLabel()
    {
        if (deleteModeLabel != null)
            deleteModeLabel.text = isDeleteMode ? "Xóa: Bật" : "Xóa: Tắt";
    }

    public void SwitchColor(Col selectedColor)
    {
        col = selectedColor;
        color = GetColorFor(selectedColor);
        isDeleteMode = false;
        UpdateDeleteModeLabel();
    }

    private void CreateColorButtons()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
            return;

        GameObject panelObject = new GameObject("Color Palette", typeof(RectTransform), typeof(Image), typeof(GridLayoutGroup));
        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.SetParent(canvas.transform, false);
        panelRect.anchorMin = new Vector2(0.5f, 1f);
        panelRect.anchorMax = new Vector2(0.5f, 1f);
        panelRect.pivot = new Vector2(0.5f, 1f);
        panelRect.anchoredPosition = new Vector2(0f, -20f);
        panelRect.sizeDelta = new Vector2(610f, 82f);

        Image panelImage = panelObject.GetComponent<Image>();
        panelImage.color = new Color(0.12f, 0.12f, 0.12f, 0.85f);

        GridLayoutGroup grid = panelObject.GetComponent<GridLayoutGroup>();
        grid.padding = new RectOffset(8, 8, 8, 8);
        grid.cellSize = new Vector2(68f, 28f);
        grid.spacing = new Vector2(4f, 4f);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 8;

        TMP_FontAsset font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        if (font == null)
            font = TMP_Settings.defaultFontAsset;

        Button initialButton = null;
        Outline initialOutline = null;
        for (int i = 0; i < System.Enum.GetValues(typeof(Col)).Length; i++)
        {
            Col selectedColor = (Col)i;
            Color swatchColor = GetColorFor(selectedColor);

            GameObject buttonObject = new GameObject($"{selectedColor} Color Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(Outline));
            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            buttonRect.SetParent(panelRect, false);

            Image buttonImage = buttonObject.GetComponent<Image>();
            buttonImage.color = swatchColor;
            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = buttonImage;

            Outline outline = buttonObject.GetComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.45f);
            outline.effectDistance = new Vector2(1f, 1f);

            button.onClick.AddListener(() => SelectColor(selectedColor, button, outline));
            CreateColorButtonLabel(buttonRect, selectedColor.ToString(), swatchColor, font);

            if (selectedColor == col)
            {
                initialButton = button;
                initialOutline = outline;
            }
        }

        SelectColor(col, initialButton, initialOutline);
    }

    private void CreateColorButtonLabel(RectTransform parent, string label, Color backgroundColor, TMP_FontAsset font)
    {
        GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.SetParent(parent, false);
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        TextMeshProUGUI text = labelObject.GetComponent<TextMeshProUGUI>();
        text.text = label;
        text.font = font;
        text.fontSize = 12f;
        text.enableAutoSizing = true;
        text.fontSizeMin = 8f;
        text.fontSizeMax = 12f;
        text.alignment = TextAlignmentOptions.Center;
        text.raycastTarget = false;
        float brightness = backgroundColor.r * 0.2126f + backgroundColor.g * 0.7152f + backgroundColor.b * 0.0722f;
        text.color = brightness > 0.55f ? Color.black : Color.white;
    }

    private void SelectColor(Col selectedColor, Button button, Outline outline)
    {
        SwitchColor(selectedColor);

        if (selectedColorOutline != null)
        {
            selectedColorOutline.effectColor = new Color(0f, 0f, 0f, 0.45f);
            selectedColorOutline.effectDistance = new Vector2(1f, 1f);
        }

        selectedColorOutline = outline;
        if (selectedColorOutline != null)
        {
            selectedColorOutline.effectColor = Color.black;
            selectedColorOutline.effectDistance = new Vector2(3f, 3f);
        }
    }

    private static Color GetColorFor(Col selectedColor)
    {
        switch (selectedColor)
        {
            case Col.white: return Color.white;
            case Col.orange: return new Color(1f, 0.5f, 0f);
            case Col.magenta: return Color.magenta;
            case Col.lightBlue: return new Color(0.35f, 0.72f, 1f);
            case Col.yellow: return Color.yellow;
            case Col.lime: return new Color(0.5f, 1f, 0f);
            case Col.pink: return new Color(1f, 0.4f, 0.7f);
            case Col.lightGray: return new Color(0.8f, 0.8f, 0.8f);
            case Col.gray: return Color.gray;
            case Col.cyan: return Color.cyan;
            case Col.purple: return new Color(0.5f, 0f, 0.5f);
            case Col.blue: return Color.blue;
            case Col.brown: return new Color(0.45f, 0.23f, 0.08f);
            case Col.green: return new Color(0f, 0.5f, 0f);
            case Col.red: return Color.red;
            case Col.black: return Color.black;
            default: return Color.white;
        }
    }
}
