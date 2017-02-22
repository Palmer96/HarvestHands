// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
//
// Custom Node Vertex Tangent World
// Donated by Community Member Kebrus
using UnityEngine;
using System;

namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "World Tangent", "Surface Standard Inputs", "Per pixel world tangent vector", null, KeyCode.None, true, false, null, null, true )]
	public sealed class VertexTangentNode : ParentNode
	{
		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddOutputVectorPorts( WirePortDataType.FLOAT3, "XYZ" );
		}

		public override void PropagateNodeData( NodeData nodeData )
		{
			base.PropagateNodeData( nodeData );
			UIUtils.CurrentDataCollector.DirtyNormal = true;
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar )
		{
			//if ( !dataCollector.DirtyNormal )
			//{
				dataCollector.ForceNormal = true;
			//}

			dataCollector.AddToInput( m_uniqueId, UIUtils.GetInputDeclarationFromType( m_currentPrecisionType, AvailableSurfaceInputs.WORLD_NORMAL ), true );
			dataCollector.AddToInput( m_uniqueId, Constants.InternalData, false );

			dataCollector.AddToLocalVariables( m_uniqueId, m_currentPrecisionType, WirePortDataType.FLOAT3, "worldTangent", "WorldNormalVector( " + Constants.InputVarStr + ", float3(1,0,0) )" );

			//dataCollector.AddToInput( m_uniqueId, "float3 worldTangent", true );
			//dataCollector.AddVertexInstruction( Constants.VertexShaderOutputStr + ".worldTangent = UnityObjectToWorldDir(" + Constants.VertexShaderInputStr + ".tangent.xyz)", m_uniqueId );

			return "worldTangent";
		}
	}
}
