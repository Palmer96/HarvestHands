// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;

namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "World Normal", "Surface Standard Inputs", "Per pixel world normal vector" )]
	public sealed class WorldNormalVector : ParentNode
	{
		private const string NormalVecValStr = "worldNormal";
		private const string NormalVecDecStr = "float3 {0} = {1};";

		//private bool m_usingNormal = false;

		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddInputPort( WirePortDataType.FLOAT3, false, "Normal" );
			AddOutputVectorPorts( WirePortDataType.FLOAT3, "XYZ" );
			//m_inputPorts[ 0 ].InternalData Vector3InternalData = UnityEngine.Vector3.forward;
			UIUtils.AddNormalDependentCount();
		}

		public override void Destroy()
		{
			base.Destroy();
			UIUtils.RemoveNormalDependentCount();
		}

		public override void PropagateNodeData( NodeData nodeData )
		{
			base.PropagateNodeData( nodeData );
			if ( m_inputPorts[ 0 ].IsConnected )
				UIUtils.CurrentDataCollector.DirtyNormal = true;
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar )
		{
			dataCollector.AddToInput( m_uniqueId, UIUtils.GetInputDeclarationFromType( m_currentPrecisionType, AvailableSurfaceInputs.WORLD_NORMAL ), true );
			dataCollector.AddToInput( m_uniqueId, Constants.InternalData, false );
			string result = string.Empty;
			if ( m_inputPorts[ 0 ].IsConnected )
			{
				result = "WorldNormalVector( " + Constants.InputVarStr + " , " + m_inputPorts[ 0 ].GenerateShaderForOutput( ref dataCollector, WirePortDataType.FLOAT3, ignoreLocalvar ) + " )";
				//if ( !dataCollector.DirtyNormal )
				//{
					dataCollector.ForceNormal = true;
				//}
			}
			else
			{
				if ( !dataCollector.DirtyNormal )
					result = Constants.InputVarStr+".worldNormal";
				else
					result = "WorldNormalVector( " + Constants.InputVarStr + ", float3(0,0,1) )";
			}

			if ( m_outputPorts[ 0 ].ConnectionCount > 1 )
			{
				dataCollector.AddToLocalVariables( m_uniqueId, string.Format( NormalVecDecStr, NormalVecValStr, result ) );
				return GetOutputVectorItem( 0, outputId, NormalVecValStr );
			}
			else
			{
				return GetOutputVectorItem( 0, outputId, result );
			}
		}
	}
}
