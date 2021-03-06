﻿using UnityEngine;

public class DebugHelper : MonoBehaviour {
    private void Start() {
        MovementLogic.FigureMoved += OnFigureMoved;
    }

    private void OnFigureMoved(GameObject arg1, (int i, int j) arg2) {
        LogFigures();
        LogTiles();
    }

    public static void LogFigures() {
        string matrix = "Figures:\n";

        for (int i = 0; i < 11; i++) {
            for (int j = 0; j < 11; j++) {
                matrix += GameMemory.Figures[i, j] ? "x   " : "-   ";
            }

            matrix += "\n";
        }

        Debug.Log(matrix);
    }

    public static void LogTiles() {
        string matrix = "Tiles:\n";

        for (int i = 0; i < 11; i++) {
            for (int j = 0; j < 11; j++) {
                matrix += GameMemory.Tiles[i, j].GetComponent<Tile>().isOccupied ? "x   " : "-   ";
            }

            matrix += "\n";
        }

        Debug.Log(matrix);
    }

    private void OnDestroy() {
        MovementLogic.FigureMoved -= OnFigureMoved;
    }
}
