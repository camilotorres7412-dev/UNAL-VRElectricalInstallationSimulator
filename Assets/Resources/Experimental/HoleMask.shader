Shader "Custom/HoleMask" 
{
    Properties {
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" } // Render before the surface
        ColorMask 0 // Makes the object invisible
        ZWrite Off  // Prevents depth occlusion issues

        Pass {
            Stencil {
                Ref [_StencilID]
                Comp Always    // Always pass the stencil test
                Pass Replace   // Write the Ref value into the buffer
            }
        }
    }
}