using System;
using UnityEngine;
[Serializable]
public class CardModel
{
    [SerializeField] private FigureType figureType;
    [SerializeField] private ColorType colorType;
    [SerializeField] private AnimalType animalType;
    public FigureType FigureType => figureType;
    public ColorType ColorType => colorType;
    public AnimalType AnimalType => animalType;

}
public enum FigureType { None, Rhoumbs, Circle, Quad}
public enum ColorType { None, Blue, Pink, Green}
public enum AnimalType { None, Bear, Panda, Wolf,  Tiger, Racoon}
