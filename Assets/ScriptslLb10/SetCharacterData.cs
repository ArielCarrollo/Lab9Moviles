using UnityEngine;
using Firebase.Firestore;
using UnityEngine.UI;
using TMPro;

namespace Assets.FirebaseScripts
{
    public class SetCharacterData : MonoBehaviour
    {
        [SerializeField] private string _characterPath = "character_sheet/one_cool_dude";
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private TMP_InputField descriptionField;
        [SerializeField] private TMP_InputField attackField;
        [SerializeField] private TMP_InputField defenseField;

        [SerializeField] private Button submitButton;

        private void OnEnable()
        {
            submitButton.onClick.AddListener(ProcessData);
        }

        private void OnDisable()
        {
            submitButton.onClick.RemoveListener(ProcessData);
        }

        private void ProcessData()
        {
            CharacterData characterData = new CharacterData
            {
                Name = nameField.text,
                Description = descriptionField.text,
                Attack = int.Parse(attackField.text),
                Defense = int.Parse(defenseField.text)
            };

            var firestore = FirebaseFirestore.DefaultInstance;

            firestore.Document(_characterPath).SetAsync(characterData);
        }
    }
}