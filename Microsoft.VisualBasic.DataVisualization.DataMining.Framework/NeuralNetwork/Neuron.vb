Imports System.Collections.Generic
Imports System.Linq

Namespace NeuralNetwork

    Public Class Neuron
#Region "-- Properties --"
        Public Property InputSynapses() As List(Of Synapse)
        Public Property OutputSynapses() As List(Of Synapse)
        Public Property Bias() As Double
        Public Property BiasDelta() As Double
        Public Property Gradient() As Double
        Public Property Value() As Double
#End Region

#Region "-- Constructors --"
        Public Sub New()
            InputSynapses = New List(Of Synapse)()
            OutputSynapses = New List(Of Synapse)()
            Bias = Helpers.GetRandom()
        End Sub

        Public Sub New(inputNeurons As IEnumerable(Of Neuron))
            Me.New()
            For Each inputNeuron As Neuron In inputNeurons
                Dim synapse = New Synapse(inputNeuron, Me)
                inputNeuron.OutputSynapses.Add(synapse)
                InputSynapses.Add(synapse)
            Next
        End Sub
#End Region

#Region "-- Values & Weights --"
        Public Overridable Function CalculateValue() As Double
            Value = Sigmoid.Output(InputSynapses.Sum(Function(a) a.Weight * a.InputNeuron.Value) + Bias)
            Return Value
        End Function

        Public Function CalculateError(target As Double) As Double
            Return target - Value
        End Function

        Public Function CalculateGradient(Optional target As System.Nullable(Of Double) = Nothing) As Double
            If target Is Nothing Then
                Gradient = OutputSynapses.Sum(Function(a) a.OutputNeuron.Gradient * a.Weight) * Sigmoid.Derivative(Value)
                Return Gradient
            Else
                Gradient = CalculateError(target.Value) * Sigmoid.Derivative(Value)
                Return Gradient
            End If
        End Function

        Public Sub UpdateWeights(learnRate As Double, momentum As Double)
            Dim prevDelta = BiasDelta
            BiasDelta = learnRate * Gradient
            Bias += BiasDelta + momentum * prevDelta

            For Each synapse As Synapse In InputSynapses
                prevDelta = synapse.WeightDelta
                synapse.WeightDelta = learnRate * Gradient * synapse.InputNeuron.Value
                synapse.Weight += synapse.WeightDelta + momentum * prevDelta
            Next
        End Sub
#End Region
    End Class
End Namespace