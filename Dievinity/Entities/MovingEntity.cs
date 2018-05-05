﻿using Dievinity.Managers;
using Dievinity.Maps;
using Dievinity.Maps.Pathing;
using Dievinity.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dievinity.Entities {
    public class MovingEntity : Entity {

        protected int speed;

        protected bool moving;
        private Point[] movementPath;

        public MovingEntity(Scene parentScene, Point position, Texture2D texture) : base(parentScene, position, texture) {
            speed = 250;

            moving = false;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (moving) {
                if (movementPath.Length > 0) {
                    if (Vector2.Distance(position, Map.GetActualPosition(movementPath[0])) <= 2.5f) {
                        position = Map.GetActualPosition(movementPath[0]);

                        List<Point> tmp = new List<Point>(movementPath);
                        tmp.RemoveAt(0);
                        movementPath = tmp.ToArray();
                    }

                    if (movementPath.Length > 0) {
                        Vector2 direction = Vector2.Normalize(Map.GetActualPosition(movementPath[0]) - position);
                        position += direction * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    } else {
                        FinishedMovement();
                    }
                } else {
                    FinishedMovement();
                }
            }
        }

        protected virtual void ExecuteMovement(Point target) {
            Point cellPosition = Map.GetCellPosition(position);
            Point targetCellPosition = target;

            PathFinder pf = new PathFinder(cellPosition, targetCellPosition, parentScene.Map);
            movementPath = pf.FindPath();
            moving = true;
        }

        protected virtual void FinishedMovement() {
            moving = false;
            movementPath = null;
        }
    }
}