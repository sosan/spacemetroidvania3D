// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/Controles.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace ControlesGlobal
{
    public class @Controles : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controles()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controles"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""5087d346-6e95-4861-95a6-28188a50cbbb"",
            ""actions"": [
                {
                    ""name"": ""mover"",
                    ""type"": ""Value"",
                    ""id"": ""1afb4fd4-1f97-4160-982a-413626b1daee"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""activar"",
                    ""type"": ""Button"",
                    ""id"": ""40da3450-192a-4790-b66c-b36c42fa3383"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""desactivar"",
                    ""type"": ""Button"",
                    ""id"": ""6c88a1dc-3587-4725-8922-e5fd30c68978"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""testeo_activar"",
                    ""type"": ""Button"",
                    ""id"": ""6e6543f2-f1aa-4f27-baf9-2c1c2eb072fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""saltar"",
                    ""type"": ""Button"",
                    ""id"": ""a1768f16-a78f-45e6-88e4-78f95982ce93"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""flamete"",
                    ""type"": ""Button"",
                    ""id"": ""33c44484-2d2c-4e98-9dce-fc75f15731c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""soldador"",
                    ""type"": ""Button"",
                    ""id"": ""758d87da-7c54-4595-a919-af0655bd3fab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ataque"",
                    ""type"": ""Button"",
                    ""id"": ""9d83acf1-5bc7-4571-99e9-4c17fec41208"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""escape"",
                    ""type"": ""Button"",
                    ""id"": ""4ba7d235-984c-4ab8-8ee7-6fa979dbe816"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""cogerObjeto"",
                    ""type"": ""Button"",
                    ""id"": ""7ef7f27a-3a44-4697-82ec-bc03e28a6082"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""magnetico"",
                    ""type"": ""Button"",
                    ""id"": ""6830dbca-dc83-4549-9fe3-1453283c6209"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""mouseposition"",
                    ""type"": ""Value"",
                    ""id"": ""08a88a89-d8ff-4a6a-8e1d-12637da47648"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Awsd"",
                    ""id"": ""546fb878-3af7-4a34-a9a8-c8fb13ef61b1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3d97bafb-2394-4b94-8abf-133651d38dac"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""065901ee-b413-4951-8109-6ab2381f8224"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Flechas"",
                    ""id"": ""03ededc0-b86d-42dc-a57f-ce1ac0ff9444"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a492b3f7-d949-48f4-b3cf-77204d8f2690"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fff36f67-b817-4a31-b5dd-610f8fd0552e"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad - Dpad"",
                    ""id"": ""21787260-570b-4a19-b04e-70a10631a1c4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""47ffa22d-090e-428d-93c1-3fbb297bb007"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""df251d4a-f08e-4786-8027-a62458511a60"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad - LeftStick"",
                    ""id"": ""6fbaf1da-fbe8-4e51-a9d8-11d7f4eb1866"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""586adfaf-dff9-441d-88c3-0d971156d040"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3823a9ad-9f34-4fba-923d-c4fa42b7b673"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c62f165a-af65-451e-a445-89c39e037e8e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""activar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abf057de-381f-4a6e-8c17-d61c8657f7f3"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""activar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0703b629-3b52-4abe-bc2c-072bf7ccf1a4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""saltar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8b4a958-ceac-4a66-a54f-a8df630c2630"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""saltar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51171523-936f-4762-81b1-540f1e46dbb3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""saltar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8438b6fb-4847-47a2-916b-4c51d10537ad"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""saltar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42fe280a-8092-422e-b586-49b9d1b55d64"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""saltar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7cf13f5-402a-4822-89f9-8ba518a004b1"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""flamete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0796219d-aa6d-4362-b7e7-113b6a26f37b"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""flamete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9587a448-153e-4a7e-9dbc-9e3a782c98f0"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""testeo_activar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95329b1b-ecae-4622-8b34-ccc9ec97d3d3"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mouseposition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93588efd-02da-4528-95ce-c6905a04ebb5"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""desactivar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ec50809-96b0-4e2e-b6b5-a62b5c8b28cd"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""desactivar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f959b44-cc32-4580-9692-9785e3b114c9"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""soldador"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53e8185d-e536-4451-b536-ed612b14f51d"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""soldador"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a96cf7f-9112-471e-9f30-6afb2c3c0854"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ataque"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1ea5d0a-e34c-480c-a28a-46a8f92fcde1"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""cogerObjeto"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1859680a-c4ab-4867-9de7-6f9610766118"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""magnetico"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""98ce8a8c-f3d4-4893-b98d-2d24c774c109"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""837029a2-acdb-4353-871d-97f9b579ef4e"",
            ""actions"": [
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""739bb71b-7be2-4fda-9353-6f092bac013c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d2728129-7c9d-46a9-be59-f244df6ec205"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3cc4824e-42cc-432a-b718-e77238f20392"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9ad0eea5-84b9-4cb8-80df-9a040ee91a92"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""378e5da2-2c17-4dfe-93df-f2440ffcf4fe"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd7dd796-4b70-4dbc-bead-e8dbc41353b4"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // PlayerMovement
            m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
            m_PlayerMovement_mover = m_PlayerMovement.FindAction("mover", throwIfNotFound: true);
            m_PlayerMovement_activar = m_PlayerMovement.FindAction("activar", throwIfNotFound: true);
            m_PlayerMovement_desactivar = m_PlayerMovement.FindAction("desactivar", throwIfNotFound: true);
            m_PlayerMovement_testeo_activar = m_PlayerMovement.FindAction("testeo_activar", throwIfNotFound: true);
            m_PlayerMovement_saltar = m_PlayerMovement.FindAction("saltar", throwIfNotFound: true);
            m_PlayerMovement_flamete = m_PlayerMovement.FindAction("flamete", throwIfNotFound: true);
            m_PlayerMovement_soldador = m_PlayerMovement.FindAction("soldador", throwIfNotFound: true);
            m_PlayerMovement_ataque = m_PlayerMovement.FindAction("ataque", throwIfNotFound: true);
            m_PlayerMovement_escape = m_PlayerMovement.FindAction("escape", throwIfNotFound: true);
            m_PlayerMovement_cogerObjeto = m_PlayerMovement.FindAction("cogerObjeto", throwIfNotFound: true);
            m_PlayerMovement_magnetico = m_PlayerMovement.FindAction("magnetico", throwIfNotFound: true);
            m_PlayerMovement_mouseposition = m_PlayerMovement.FindAction("mouseposition", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
            m_UI_LeftClick = m_UI.FindAction("LeftClick", throwIfNotFound: true);
            m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // PlayerMovement
        private readonly InputActionMap m_PlayerMovement;
        private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
        private readonly InputAction m_PlayerMovement_mover;
        private readonly InputAction m_PlayerMovement_activar;
        private readonly InputAction m_PlayerMovement_desactivar;
        private readonly InputAction m_PlayerMovement_testeo_activar;
        private readonly InputAction m_PlayerMovement_saltar;
        private readonly InputAction m_PlayerMovement_flamete;
        private readonly InputAction m_PlayerMovement_soldador;
        private readonly InputAction m_PlayerMovement_ataque;
        private readonly InputAction m_PlayerMovement_escape;
        private readonly InputAction m_PlayerMovement_cogerObjeto;
        private readonly InputAction m_PlayerMovement_magnetico;
        private readonly InputAction m_PlayerMovement_mouseposition;
        public struct PlayerMovementActions
        {
            private @Controles m_Wrapper;
            public PlayerMovementActions(@Controles wrapper) { m_Wrapper = wrapper; }
            public InputAction @mover => m_Wrapper.m_PlayerMovement_mover;
            public InputAction @activar => m_Wrapper.m_PlayerMovement_activar;
            public InputAction @desactivar => m_Wrapper.m_PlayerMovement_desactivar;
            public InputAction @testeo_activar => m_Wrapper.m_PlayerMovement_testeo_activar;
            public InputAction @saltar => m_Wrapper.m_PlayerMovement_saltar;
            public InputAction @flamete => m_Wrapper.m_PlayerMovement_flamete;
            public InputAction @soldador => m_Wrapper.m_PlayerMovement_soldador;
            public InputAction @ataque => m_Wrapper.m_PlayerMovement_ataque;
            public InputAction @escape => m_Wrapper.m_PlayerMovement_escape;
            public InputAction @cogerObjeto => m_Wrapper.m_PlayerMovement_cogerObjeto;
            public InputAction @magnetico => m_Wrapper.m_PlayerMovement_magnetico;
            public InputAction @mouseposition => m_Wrapper.m_PlayerMovement_mouseposition;
            public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerMovementActions instance)
            {
                if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
                {
                    @mover.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMover;
                    @mover.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMover;
                    @mover.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMover;
                    @activar.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnActivar;
                    @activar.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnActivar;
                    @activar.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnActivar;
                    @desactivar.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDesactivar;
                    @desactivar.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDesactivar;
                    @desactivar.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDesactivar;
                    @testeo_activar.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnTesteo_activar;
                    @testeo_activar.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnTesteo_activar;
                    @testeo_activar.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnTesteo_activar;
                    @saltar.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSaltar;
                    @saltar.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSaltar;
                    @saltar.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSaltar;
                    @flamete.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnFlamete;
                    @flamete.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnFlamete;
                    @flamete.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnFlamete;
                    @soldador.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSoldador;
                    @soldador.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSoldador;
                    @soldador.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSoldador;
                    @ataque.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnAtaque;
                    @ataque.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnAtaque;
                    @ataque.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnAtaque;
                    @escape.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnEscape;
                    @escape.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnEscape;
                    @escape.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnEscape;
                    @cogerObjeto.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCogerObjeto;
                    @cogerObjeto.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCogerObjeto;
                    @cogerObjeto.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCogerObjeto;
                    @magnetico.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMagnetico;
                    @magnetico.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMagnetico;
                    @magnetico.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMagnetico;
                    @mouseposition.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMouseposition;
                    @mouseposition.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMouseposition;
                    @mouseposition.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMouseposition;
                }
                m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @mover.started += instance.OnMover;
                    @mover.performed += instance.OnMover;
                    @mover.canceled += instance.OnMover;
                    @activar.started += instance.OnActivar;
                    @activar.performed += instance.OnActivar;
                    @activar.canceled += instance.OnActivar;
                    @desactivar.started += instance.OnDesactivar;
                    @desactivar.performed += instance.OnDesactivar;
                    @desactivar.canceled += instance.OnDesactivar;
                    @testeo_activar.started += instance.OnTesteo_activar;
                    @testeo_activar.performed += instance.OnTesteo_activar;
                    @testeo_activar.canceled += instance.OnTesteo_activar;
                    @saltar.started += instance.OnSaltar;
                    @saltar.performed += instance.OnSaltar;
                    @saltar.canceled += instance.OnSaltar;
                    @flamete.started += instance.OnFlamete;
                    @flamete.performed += instance.OnFlamete;
                    @flamete.canceled += instance.OnFlamete;
                    @soldador.started += instance.OnSoldador;
                    @soldador.performed += instance.OnSoldador;
                    @soldador.canceled += instance.OnSoldador;
                    @ataque.started += instance.OnAtaque;
                    @ataque.performed += instance.OnAtaque;
                    @ataque.canceled += instance.OnAtaque;
                    @escape.started += instance.OnEscape;
                    @escape.performed += instance.OnEscape;
                    @escape.canceled += instance.OnEscape;
                    @cogerObjeto.started += instance.OnCogerObjeto;
                    @cogerObjeto.performed += instance.OnCogerObjeto;
                    @cogerObjeto.canceled += instance.OnCogerObjeto;
                    @magnetico.started += instance.OnMagnetico;
                    @magnetico.performed += instance.OnMagnetico;
                    @magnetico.canceled += instance.OnMagnetico;
                    @mouseposition.started += instance.OnMouseposition;
                    @mouseposition.performed += instance.OnMouseposition;
                    @mouseposition.canceled += instance.OnMouseposition;
                }
            }
        }
        public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private IUIActions m_UIActionsCallbackInterface;
        private readonly InputAction m_UI_Point;
        private readonly InputAction m_UI_LeftClick;
        private readonly InputAction m_UI_RightClick;
        public struct UIActions
        {
            private @Controles m_Wrapper;
            public UIActions(@Controles wrapper) { m_Wrapper = wrapper; }
            public InputAction @Point => m_Wrapper.m_UI_Point;
            public InputAction @LeftClick => m_Wrapper.m_UI_LeftClick;
            public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void SetCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterface != null)
                {
                    @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                    @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                    @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                    @LeftClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnLeftClick;
                    @LeftClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnLeftClick;
                    @LeftClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnLeftClick;
                    @RightClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                    @RightClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                    @RightClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                }
                m_Wrapper.m_UIActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Point.started += instance.OnPoint;
                    @Point.performed += instance.OnPoint;
                    @Point.canceled += instance.OnPoint;
                    @LeftClick.started += instance.OnLeftClick;
                    @LeftClick.performed += instance.OnLeftClick;
                    @LeftClick.canceled += instance.OnLeftClick;
                    @RightClick.started += instance.OnRightClick;
                    @RightClick.performed += instance.OnRightClick;
                    @RightClick.canceled += instance.OnRightClick;
                }
            }
        }
        public UIActions @UI => new UIActions(this);
        public interface IPlayerMovementActions
        {
            void OnMover(InputAction.CallbackContext context);
            void OnActivar(InputAction.CallbackContext context);
            void OnDesactivar(InputAction.CallbackContext context);
            void OnTesteo_activar(InputAction.CallbackContext context);
            void OnSaltar(InputAction.CallbackContext context);
            void OnFlamete(InputAction.CallbackContext context);
            void OnSoldador(InputAction.CallbackContext context);
            void OnAtaque(InputAction.CallbackContext context);
            void OnEscape(InputAction.CallbackContext context);
            void OnCogerObjeto(InputAction.CallbackContext context);
            void OnMagnetico(InputAction.CallbackContext context);
            void OnMouseposition(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnPoint(InputAction.CallbackContext context);
            void OnLeftClick(InputAction.CallbackContext context);
            void OnRightClick(InputAction.CallbackContext context);
        }
    }
}
