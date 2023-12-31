//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Scripts/OurInputs.inputactions
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

public partial class @OurInputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @OurInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""OurInputs"",
    ""maps"": [
        {
            ""name"": ""Core"",
            ""id"": ""229f24fd-92a5-49a8-a4ef-d0327b852fe8"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b0d68ec9-45f4-46c6-b2fd-1704355e4da5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""8412d055-0c03-4fe2-a873-ca90454f2a06"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CameraMouse"",
                    ""type"": ""Value"",
                    ""id"": ""5df507ae-e37a-4ab2-93d2-acc58e4e2c71"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a038f3e5-a6b6-4f64-b0a4-1b4bf822b0a8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""da133792-7e94-4a1f-a2b2-891cf81b98dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Hit"",
                    ""type"": ""Button"",
                    ""id"": ""3a84f5ea-d444-49a1-9989-2b478e146088"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Binocucom"",
                    ""type"": ""Button"",
                    ""id"": ""d81d090c-f2a8-4cc2-9853-8519161c027b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseCamera"",
                    ""type"": ""Value"",
                    ""id"": ""6b8f4a71-2a6e-44ff-a814-f9f9fa27c933"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""FovDelta"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d9ec08de-f27d-47cb-b762-eb57a4dcdf4c"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""24002e9e-a085-46af-82cc-5bf1ad5da6c0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""fa8934d0-d001-42f5-a04a-7e3a8c86e066"",
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
                    ""id"": ""3e427b58-3c7c-4656-8f17-0d5d22f0147d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""66563ae0-f1a2-424e-9e5f-3233353ebaf5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c0c638f8-2f52-4c0b-b038-75da02ab1100"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""399bde96-38b6-4575-b566-6658e399385b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""4eeed4cf-30d1-471f-8ab7-822b62318228"",
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
                    ""id"": ""3ddbdd4e-52a0-46d4-89dc-9eea4af92e5d"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e1687af0-bcb6-4c53-b20f-0f768af3df02"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2ba9b4b3-dfbc-4836-9f31-5b7a58e948e6"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fa9391fa-a45d-4590-8b25-1132639eb5d0"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f523784e-a7da-48b7-bf37-52c614ef1821"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""CameraMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f039b953-d794-4fac-a3db-cd479aebfde1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f708d2b2-1703-48f7-9477-3c4f602ff55a"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a68eaca6-258a-45f6-9cfa-9acd61da971e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6714b414-d693-46a6-a7f0-e5e76a2cefef"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""58a50a37-fcbd-487a-a8ff-bff75793570c"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9b18526d-e7e2-4904-b43f-042c0b1eb7c9"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d412d4ce-d183-47ed-8e47-2a39ba832285"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Hit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""518f6b9d-054a-4f38-a8c2-56ad5cb0bcd8"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Hit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""251f16ac-4115-4488-acc0-e748a680963c"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""Binocucom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9435f03d-64ca-4ff1-a5f4-57e42a2ed76d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""MouseCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d33cfaa4-268d-4988-a620-44de8d5224d5"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""FovDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""f7212c19-816b-4fe2-95d5-50cd943aba49"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FovDelta"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f3fb79f1-8021-4a4a-b0d3-396deabcab71"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""FovDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""aa3de926-5c1d-4ddb-83a7-e7bd3a063d8f"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Core"",
                    ""action"": ""FovDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Core"",
            ""bindingGroup"": ""Core"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Core
        m_Core = asset.FindActionMap("Core", throwIfNotFound: true);
        m_Core_Jump = m_Core.FindAction("Jump", throwIfNotFound: true);
        m_Core_Move = m_Core.FindAction("Move", throwIfNotFound: true);
        m_Core_CameraMouse = m_Core.FindAction("CameraMouse", throwIfNotFound: true);
        m_Core_Camera = m_Core.FindAction("Camera", throwIfNotFound: true);
        m_Core_Interact = m_Core.FindAction("Interact", throwIfNotFound: true);
        m_Core_Hit = m_Core.FindAction("Hit", throwIfNotFound: true);
        m_Core_Binocucom = m_Core.FindAction("Binocucom", throwIfNotFound: true);
        m_Core_MouseCamera = m_Core.FindAction("MouseCamera", throwIfNotFound: true);
        m_Core_FovDelta = m_Core.FindAction("FovDelta", throwIfNotFound: true);
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

    // Core
    private readonly InputActionMap m_Core;
    private List<ICoreActions> m_CoreActionsCallbackInterfaces = new List<ICoreActions>();
    private readonly InputAction m_Core_Jump;
    private readonly InputAction m_Core_Move;
    private readonly InputAction m_Core_CameraMouse;
    private readonly InputAction m_Core_Camera;
    private readonly InputAction m_Core_Interact;
    private readonly InputAction m_Core_Hit;
    private readonly InputAction m_Core_Binocucom;
    private readonly InputAction m_Core_MouseCamera;
    private readonly InputAction m_Core_FovDelta;
    public struct CoreActions
    {
        private @OurInputs m_Wrapper;
        public CoreActions(@OurInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Core_Jump;
        public InputAction @Move => m_Wrapper.m_Core_Move;
        public InputAction @CameraMouse => m_Wrapper.m_Core_CameraMouse;
        public InputAction @Camera => m_Wrapper.m_Core_Camera;
        public InputAction @Interact => m_Wrapper.m_Core_Interact;
        public InputAction @Hit => m_Wrapper.m_Core_Hit;
        public InputAction @Binocucom => m_Wrapper.m_Core_Binocucom;
        public InputAction @MouseCamera => m_Wrapper.m_Core_MouseCamera;
        public InputAction @FovDelta => m_Wrapper.m_Core_FovDelta;
        public InputActionMap Get() { return m_Wrapper.m_Core; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CoreActions set) { return set.Get(); }
        public void AddCallbacks(ICoreActions instance)
        {
            if (instance == null || m_Wrapper.m_CoreActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CoreActionsCallbackInterfaces.Add(instance);
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @CameraMouse.started += instance.OnCameraMouse;
            @CameraMouse.performed += instance.OnCameraMouse;
            @CameraMouse.canceled += instance.OnCameraMouse;
            @Camera.started += instance.OnCamera;
            @Camera.performed += instance.OnCamera;
            @Camera.canceled += instance.OnCamera;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @Hit.started += instance.OnHit;
            @Hit.performed += instance.OnHit;
            @Hit.canceled += instance.OnHit;
            @Binocucom.started += instance.OnBinocucom;
            @Binocucom.performed += instance.OnBinocucom;
            @Binocucom.canceled += instance.OnBinocucom;
            @MouseCamera.started += instance.OnMouseCamera;
            @MouseCamera.performed += instance.OnMouseCamera;
            @MouseCamera.canceled += instance.OnMouseCamera;
            @FovDelta.started += instance.OnFovDelta;
            @FovDelta.performed += instance.OnFovDelta;
            @FovDelta.canceled += instance.OnFovDelta;
        }

        private void UnregisterCallbacks(ICoreActions instance)
        {
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @CameraMouse.started -= instance.OnCameraMouse;
            @CameraMouse.performed -= instance.OnCameraMouse;
            @CameraMouse.canceled -= instance.OnCameraMouse;
            @Camera.started -= instance.OnCamera;
            @Camera.performed -= instance.OnCamera;
            @Camera.canceled -= instance.OnCamera;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @Hit.started -= instance.OnHit;
            @Hit.performed -= instance.OnHit;
            @Hit.canceled -= instance.OnHit;
            @Binocucom.started -= instance.OnBinocucom;
            @Binocucom.performed -= instance.OnBinocucom;
            @Binocucom.canceled -= instance.OnBinocucom;
            @MouseCamera.started -= instance.OnMouseCamera;
            @MouseCamera.performed -= instance.OnMouseCamera;
            @MouseCamera.canceled -= instance.OnMouseCamera;
            @FovDelta.started -= instance.OnFovDelta;
            @FovDelta.performed -= instance.OnFovDelta;
            @FovDelta.canceled -= instance.OnFovDelta;
        }

        public void RemoveCallbacks(ICoreActions instance)
        {
            if (m_Wrapper.m_CoreActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICoreActions instance)
        {
            foreach (var item in m_Wrapper.m_CoreActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CoreActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CoreActions @Core => new CoreActions(this);
    private int m_CoreSchemeIndex = -1;
    public InputControlScheme CoreScheme
    {
        get
        {
            if (m_CoreSchemeIndex == -1) m_CoreSchemeIndex = asset.FindControlSchemeIndex("Core");
            return asset.controlSchemes[m_CoreSchemeIndex];
        }
    }
    public interface ICoreActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnCameraMouse(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnHit(InputAction.CallbackContext context);
        void OnBinocucom(InputAction.CallbackContext context);
        void OnMouseCamera(InputAction.CallbackContext context);
        void OnFovDelta(InputAction.CallbackContext context);
    }
}
