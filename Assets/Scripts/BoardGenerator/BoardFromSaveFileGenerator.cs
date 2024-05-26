using System;
using System.Collections.Generic;
using Board;
using Configs;
using Saves;
using Solver;

namespace BoardGenerator
{
	public class BoardFromSaveFileGenerator : IBoardGenerator
	{
		private readonly SudokuGridConfig _sudokuGridConfig;
		private readonly IReadOnlyList<SerializableCell> _serializableCells;

		public BoardFromSaveFileGenerator(SudokuGridConfig sudokuGridConfig, IReadOnlyList<SerializableCell> serializableCells)
		{
			_sudokuGridConfig = sudokuGridConfig;
			_serializableCells = serializableCells;
		}

		public void Generate(Board.Board board)
		{
			foreach (SerializableCell serializableCell in _serializableCells)
			{
				switch (serializableCell.CellType)
				{
					case CellType.EMPTY:
						board.SetCellAsEmpty(serializableCell.Index, serializableCell.GroupBox, serializableCell.Row, serializableCell.Column);
						break;
					case CellType.SOLVER:
						board.SetCellAsSolver(serializableCell.Index, serializableCell.GroupBox, serializableCell.Row, serializableCell.Column, serializableCell.Number);
						break;
					case CellType.USER:
						board.SetCellAsUser(serializableCell.Index, serializableCell.GroupBox, serializableCell.Row, serializableCell.Column, serializableCell.Number, serializableCell.IsPlacedGood);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}