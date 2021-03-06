﻿using System;
using System.Collections.Generic;
using Tags;
using UnityEngine;

public static class MovementLogic {
    private static readonly List<Tile> HighlightedTiles = new List<Tile>();

    public static Action<GameObject, (int i, int j)> FigureMoved;

    public static void MovePiece(GameObject figure, GameObject destinationTile) {
        // restrict movement to occupied tile
        if (destinationTile.GetComponent<Tile>().isOccupied) return;

        // note: figures and tiles matrices have same indexes
        var origin = GameMemory.GetIndices(figure);
        var dest = GameMemory.GetIndices(destinationTile);

        // a piece may move horizontally or vertically but not diagonally
        if (dest.Item1 != origin.Item1 && dest.Item2 != origin.Item2) return;

        // change figure postion
        figure.GetComponent<Piece>().MovePiece(destinationTile);

        // finally, clear highlights
        resetHighlight();
    }

    public static void HighlightViableMoves(GameObject figure) {
        var origin = GameMemory.GetIndices(figure);
        bool isKing = figure.CompareTag(FigureTags.King);

        Action<GameObject> ColorTile = (GameObject tile) => {
            // only the King may visit the refugee tiles
            if ((tile.CompareTag(TileTags.Haven) || tile.CompareTag(TileTags.King)) && isKing == false) return;

            var highlighted = tile.GetComponent<Tile>();

            highlighted.ToggleHighlight(true);
            HighlightedTiles.Add(highlighted);
        };

        // reset any existing highlighting
        resetHighlight();

        // vertical - down
        for (int i = origin.Item1 + 1; i < 11; i++) {
            var tile = GameMemory.Tiles[i, origin.Item2];

            if (tile.GetComponent<Tile>().isOccupied) break;

            ColorTile(tile);
        }

        // vertial - up
        for (int i = origin.Item1 - 1; i >= 0; i--) {
            var tile = GameMemory.Tiles[i, origin.Item2];

            if (tile.GetComponent<Tile>().isOccupied) break;

            ColorTile(tile);
        }

        // horizontal - left
        for (int j = origin.Item2 + 1; j < 11; j++) {
            var tile = GameMemory.Tiles[origin.Item1, j];

            if (tile.GetComponent<Tile>().isOccupied) break;

            ColorTile(tile);
        }

        // horizontal - right
        for (int j = origin.Item2 - 1; j >= 0; j--) {
            var tile = GameMemory.Tiles[origin.Item1, j];

            if (tile.GetComponent<Tile>().isOccupied) break;

            ColorTile(tile);
        }
    }

    public static void resetHighlight() {
        HighlightedTiles.ForEach(tile => tile.ToggleHighlight(false));
        HighlightedTiles.Clear();
    }
}
