//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputSystem/MyInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @MyInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MyInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MyInputActions"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""5757ab1b-04d0-4782-a6b4-029973bd3846"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a29dfd88-989c-4080-9231-9b025e373166"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e476aec3-09c2-41fc-a247-2f01168cea37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""5b0fcbff-d2ea-4965-be95-de25685394e9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Scope"",
                    ""type"": ""Button"",
                    ""id"": ""84516fd6-89e4-4c11-8e5e-298e22a59ad2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseLock"",
                    ""type"": ""Button"",
                    ""id"": ""7d6b7518-a52f-4508-8b81-32b69c64490f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""2f74d277-8ddf-4a1c-9ec3-06ed1bf25cee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""3e47f2a1-d880-4a56-a78a-aff9e5df6989"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""c98dfb00-dbd4-4fa9-be94-e1ffef892e4d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Respawn"",
                    ""type"": ""Button"",
                    ""id"": ""306c2471-d978-4d52-870e-71915740db7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ready Up"",
                    ""type"": ""Button"",
                    ""id"": ""5579e74e-6f80-432a-9bcb-58148dea84ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Create Player"",
                    ""type"": ""Button"",
                    ""id"": ""22bea428-ee78-4823-b13d-4ff7d475f6ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""3d68613b-8fc6-41c5-bef0-7e179bcf712a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""942f9e6c-a4ec-4a93-be9f-52d6f6048e39"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""c72ef905-e40b-4d25-a4b8-c65895aea807"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5cbd2bcc-dd99-4b85-b0e4-4f3611c5ef8f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""33adaa3f-1dcb-412d-8e94-8e69fc925100"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""566ce3f3-8854-4cd5-90ad-d75c68ccc81c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0e9f34e0-4f3e-4e8a-a1b1-0d3f9242166e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""beff9efa-ad34-41a4-a0fa-b650760afc0e"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac2a8443-d5a2-410a-9cc8-b1e45507e412"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""275752e0-d5e8-4838-88f4-8eedeae432c7"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.07,y=0.07)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed97e843-4421-4386-9e56-755b83589589"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.7,y=0.7)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e44e3f7a-153a-4b0b-ab57-cf64a14c1ed8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scope"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b95f573e-0295-47c8-8880-893ca6da44f1"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scope"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2564a725-6bc6-4873-bfbf-4b0881228c82"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62323841-ee5d-424a-ad5f-e66e8c498a84"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ebd6ee6-8315-48dd-98f2-791b1e3065db"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9318d622-09fb-46d6-8fd9-8d2711b95022"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ee724ad-139e-4754-8f79-4fd2ac358dc2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55132ddf-8a2e-4e48-9015-49b1b470bfe2"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c571db42-3984-4d9b-9850-8e6d28a65a7a"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09c4d7a0-b3e2-4282-898d-4b69f5f7bd3b"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e694082-82cf-4275-92b1-c382cdc32581"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Respawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""896352b0-ea7a-43d8-a2ab-5923a67e9548"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Respawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8bd8c2e9-6011-45d9-aab8-0f5336725517"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ready Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b8ad140-66d5-4a31-81e6-8158e8132744"",
                    ""path"": ""<Keyboard>/rightBracket"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Create Player"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d135db82-1021-4344-bb76-b3cc2d73e245"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_Move = m_Main.FindAction("Move", throwIfNotFound: true);
        m_Main_Jump = m_Main.FindAction("Jump", throwIfNotFound: true);
        m_Main_Look = m_Main.FindAction("Look", throwIfNotFound: true);
        m_Main_Scope = m_Main.FindAction("Scope", throwIfNotFound: true);
        m_Main_MouseLock = m_Main.FindAction("MouseLock", throwIfNotFound: true);
        m_Main_Fire = m_Main.FindAction("Fire", throwIfNotFound: true);
        m_Main_Drop = m_Main.FindAction("Drop", throwIfNotFound: true);
        m_Main_Crouch = m_Main.FindAction("Crouch", throwIfNotFound: true);
        m_Main_Respawn = m_Main.FindAction("Respawn", throwIfNotFound: true);
        m_Main_ReadyUp = m_Main.FindAction("Ready Up", throwIfNotFound: true);
        m_Main_CreatePlayer = m_Main.FindAction("Create Player", throwIfNotFound: true);
        m_Main_Submit = m_Main.FindAction("Submit", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_Move;
    private readonly InputAction m_Main_Jump;
    private readonly InputAction m_Main_Look;
    private readonly InputAction m_Main_Scope;
    private readonly InputAction m_Main_MouseLock;
    private readonly InputAction m_Main_Fire;
    private readonly InputAction m_Main_Drop;
    private readonly InputAction m_Main_Crouch;
    private readonly InputAction m_Main_Respawn;
    private readonly InputAction m_Main_ReadyUp;
    private readonly InputAction m_Main_CreatePlayer;
    private readonly InputAction m_Main_Submit;
    public struct MainActions
    {
        private @MyInputActions m_Wrapper;
        public MainActions(@MyInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Main_Move;
        public InputAction @Jump => m_Wrapper.m_Main_Jump;
        public InputAction @Look => m_Wrapper.m_Main_Look;
        public InputAction @Scope => m_Wrapper.m_Main_Scope;
        public InputAction @MouseLock => m_Wrapper.m_Main_MouseLock;
        public InputAction @Fire => m_Wrapper.m_Main_Fire;
        public InputAction @Drop => m_Wrapper.m_Main_Drop;
        public InputAction @Crouch => m_Wrapper.m_Main_Crouch;
        public InputAction @Respawn => m_Wrapper.m_Main_Respawn;
        public InputAction @ReadyUp => m_Wrapper.m_Main_ReadyUp;
        public InputAction @CreatePlayer => m_Wrapper.m_Main_CreatePlayer;
        public InputAction @Submit => m_Wrapper.m_Main_Submit;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_MainActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnJump;
                @Look.started -= m_Wrapper.m_MainActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnLook;
                @Scope.started -= m_Wrapper.m_MainActionsCallbackInterface.OnScope;
                @Scope.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnScope;
                @Scope.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnScope;
                @MouseLock.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMouseLock;
                @MouseLock.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMouseLock;
                @MouseLock.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMouseLock;
                @Fire.started -= m_Wrapper.m_MainActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnFire;
                @Drop.started -= m_Wrapper.m_MainActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnDrop;
                @Crouch.started -= m_Wrapper.m_MainActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnCrouch;
                @Respawn.started -= m_Wrapper.m_MainActionsCallbackInterface.OnRespawn;
                @Respawn.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnRespawn;
                @Respawn.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnRespawn;
                @ReadyUp.started -= m_Wrapper.m_MainActionsCallbackInterface.OnReadyUp;
                @ReadyUp.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnReadyUp;
                @ReadyUp.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnReadyUp;
                @CreatePlayer.started -= m_Wrapper.m_MainActionsCallbackInterface.OnCreatePlayer;
                @CreatePlayer.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnCreatePlayer;
                @CreatePlayer.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnCreatePlayer;
                @Submit.started -= m_Wrapper.m_MainActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnSubmit;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Scope.started += instance.OnScope;
                @Scope.performed += instance.OnScope;
                @Scope.canceled += instance.OnScope;
                @MouseLock.started += instance.OnMouseLock;
                @MouseLock.performed += instance.OnMouseLock;
                @MouseLock.canceled += instance.OnMouseLock;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Respawn.started += instance.OnRespawn;
                @Respawn.performed += instance.OnRespawn;
                @Respawn.canceled += instance.OnRespawn;
                @ReadyUp.started += instance.OnReadyUp;
                @ReadyUp.performed += instance.OnReadyUp;
                @ReadyUp.canceled += instance.OnReadyUp;
                @CreatePlayer.started += instance.OnCreatePlayer;
                @CreatePlayer.performed += instance.OnCreatePlayer;
                @CreatePlayer.canceled += instance.OnCreatePlayer;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
            }
        }
    }
    public MainActions @Main => new MainActions(this);
    public interface IMainActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnScope(InputAction.CallbackContext context);
        void OnMouseLock(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnRespawn(InputAction.CallbackContext context);
        void OnReadyUp(InputAction.CallbackContext context);
        void OnCreatePlayer(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
    }
}
