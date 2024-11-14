using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace ItemsDisplay
{
    public class ItemsScreen : MonoBehaviour
    {
        [SerializeField] private List<ItemObject> itemObjects;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private List<CategorySprite> categorySprites;
        [SerializeField] private GameObject loadingObject;

        [Inject] private IDataServer _dataServer;
        private int _currentIndex;
        
        private CancellationTokenSource _cancellationTokenSource;
        private List<DataItem> _dataItems = new List<DataItem>();
        
        private const int DATA_DISPLAY_PER_PAGE = 5;
        
        private void OnEnable()
        {
            previousButton.onClick.AddListener(OnPreviousButtonClicked);
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }

        private void OnDisable()
        {
            previousButton.onClick.RemoveListener(OnPreviousButtonClicked);
            nextButton.onClick.RemoveListener(OnNextButtonClicked);
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            UpdateVisuals();
            LoadInitialData();
        }

        private async void LoadInitialData()
        {
            var totalItems = await _dataServer.DataAvailable(_cancellationTokenSource.Token);
            _dataItems = (List<DataItem>)await _dataServer.RequestData(0, totalItems, _cancellationTokenSource.Token);
            loadingObject.SetActive(false);
            UpdateVisuals();
            DisplayPage();
        }
        
        private void DisplayPage()
        {
            ClearCurrentItems();
            
            for (var i = 0; i < DATA_DISPLAY_PER_PAGE; i++)
            {
                var dataItemIndex = i + (_currentIndex * DATA_DISPLAY_PER_PAGE);
                if (dataItemIndex >= _dataItems.Count)
                {
                    return;
                }
                var dataItem = _dataItems[dataItemIndex];
                var itemObject = itemObjects[i];
                var badgeSprite = categorySprites.FirstOrDefault(p => p.CategoryType == dataItem.Category)?.Sprite;
                
                itemObject.UpdateVisuals(dataItemIndex + 1, dataItem.Description, badgeSprite, dataItem.Special);
                itemObject.gameObject.SetActive(true);
            }
        }

        private void ClearCurrentItems()
        {
            foreach (var itemObject in itemObjects)
            {
                itemObject.gameObject.SetActive(false);
            }
        }
        
        private void OnPreviousButtonClicked()
        {
            _currentIndex--;
            DisplayPage();
            UpdateVisuals();
        }

        private void OnNextButtonClicked()
        {
            _currentIndex++;
            DisplayPage();
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            previousButton.interactable = _currentIndex > 0;
            var endIndex = Mathf.Min((_currentIndex + 1) * DATA_DISPLAY_PER_PAGE, _dataItems.Count);
            nextButton.interactable = endIndex < _dataItems.Count;
        }
    }
}