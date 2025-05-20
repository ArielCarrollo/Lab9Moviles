using UnityEngine;
using Firebase.Firestore;
using UnityEngine.UI;
using TMPro;
using Firebase.Extensions;
using NUnit.Framework;

namespace Assets.FirebaseScripts
{
    public class GetCharacterData : MonoBehaviour
    {
        [SerializeField] private string _characterPath = "character_sheet/one_cool_dude";
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text defenseText;

        [SerializeField] private Button submitButton;

        //private ListenerRegistration _listenerRegistration;

        private void Start()
        {
            var firestore = FirebaseFirestore.DefaultInstance;

            /*_listenerRegistration = firestore.Document(_characterPath).Listen(snapshot => {

                CharacterData characterData = snapshot.ConvertTo<CharacterData>();

                nameText.text = $"Name: {characterData.Name}";
                descriptionText.text = $"Decription: {characterData.Description}";
                attackText.text = $"Attack: {characterData.Attack.ToString()}";
                defenseText.text = $"Defense: {characterData.Defense.ToString()}";
            });*/
        }

        private void OnEnable()
        {
            submitButton.onClick.AddListener(ProcessData);
        }

        private void OnDisable()
        {
            submitButton.onClick.RemoveListener(ProcessData);
        }

        private void OnDestroy()
        {
            //_listenerRegistration.Stop();
        }

        public void ProcessData()
        {
            var firestore = FirebaseFirestore.DefaultInstance;

            firestore.Document(_characterPath).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                Assert.IsNotNull(task.Exception);

                CharacterData characterData = task.Result.ConvertTo<CharacterData>();

                nameText.text = $"Name: {characterData.Name}";
                descriptionText.text = $"Decription: {characterData.Description}";
                attackText.text = $"Attack: {characterData.Attack.ToString()}";
                defenseText.text = $"Defense: {characterData.Defense.ToString()}";
            });
        }
    }
}