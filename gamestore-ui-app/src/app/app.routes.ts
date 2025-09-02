import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { GamesComponent } from './components/games/games.component';
import { GameComponent } from './components/game/game.component';
import { AddGameComponent } from './components/add-game/add-game.component';
import { UpdateGameComponent } from './components/update-game/update-game.component';
import { GenresComponent } from './components/genres/genres.component';
import { GenreComponent } from './components/genre/genre.component';
import { AddGenreComponent } from './components/add-genre/add-genre.component';
import { UpdateGenreComponent } from './components/update-genre/update-genre.component';
import { PlatformsComponent } from './components/platforms/platforms.component';
import { PlatformComponent } from './components/platform/platform.component';
import { AddPlatformComponent } from './components/add-platform/add-platform.component';
import { UpdatePlatformComponent } from './components/update-platform/update-platform.component';
import { PublishersComponent } from './components/publishers/publishers.component';
import { PublisherComponent } from './components/publisher/publisher.component';
import { AddPublisherComponent } from './components/add-publisher/add-publisher.component';
import { UpdatePublisherComponent } from './components/update-publisher/update-publisher.component';
import { OrdersComponent } from './components/orders/orders.component';
import { OrderComponent } from './components/order/order.component';
import { UpdateOrderComponent } from './components/update-order/update-order.component';
import { HistoryComponent } from './components/history/history.component';
import { BasketComponent } from './components/basket/basket.component';
import { MakeOrderComponent } from './components/make-order/make-order.component';
import { UsersComponent } from './components/users/users.component';
import { UserComponent } from './components/user/user.component';
import { AddUserComponent } from './components/add-user/add-user.component';
import { UpdateUserComponent } from './components/update-user/update-user.component';
import { LoginComponent } from './components/login/login.component';
import { RolesComponent } from './components/roles/roles.component';
import { RoleComponent } from './components/role/role.component';
import { AddRoleComponent } from './components/add-role/add-role.component';
import { UpdateRoleComponent } from './components/update-role/update-role.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  
  // Game routes
  { path: 'games', component: GamesComponent },
  { path: 'game', component: GameComponent },
  { path: 'game/:id', component: GameComponent },
  { path: 'add-game', component: AddGameComponent },
  { path: 'update-game', component: UpdateGameComponent },
  { path: 'update-game/:id', component: UpdateGameComponent },
  { path: 'delete-game/:id', redirectTo: '/games' },
  
  // Genre routes
  { path: 'genres', component: GenresComponent },
  { path: 'genre', component: GenreComponent },
  { path: 'genre/:id', component: GenreComponent },
  { path: 'add-genre', component: AddGenreComponent },
  { path: 'update-genre', component: UpdateGenreComponent },
  { path: 'update-genre/:id', component: UpdateGenreComponent },
  { path: 'delete-genre/:id', redirectTo: '/genres' },
  
  // Platform routes
  { path: 'platforms', component: PlatformsComponent },
  { path: 'platform', component: PlatformComponent },
  { path: 'platform/:id', component: PlatformComponent },
  { path: 'add-platform', component: AddPlatformComponent },
  { path: 'update-platform', component: UpdatePlatformComponent },
  { path: 'update-platform/:id', component: UpdatePlatformComponent },
  { path: 'delete-platform/:id', redirectTo: '/platforms' },
  
  // Publisher routes
  { path: 'publishers', component: PublishersComponent },
  { path: 'publisher', component: PublisherComponent },
  { path: 'publisher/:id', component: PublisherComponent },
  { path: 'add-publisher', component: AddPublisherComponent },
  { path: 'update-publisher', component: UpdatePublisherComponent },
  { path: 'update-publisher/:id', component: UpdatePublisherComponent },
  { path: 'delete-publisher/:id', redirectTo: '/publishers' },
  
  // Order routes
  { path: 'orders', component: OrdersComponent },
  { path: 'order', component: MakeOrderComponent },
  { path: 'order/:id', component: OrderComponent },
  { path: 'update-order', component: UpdateOrderComponent },
  { path: 'update-order/:id', component: UpdateOrderComponent },
  { path: 'history', component: HistoryComponent },
  { path: 'basket', component: BasketComponent },
  
  // User routes
  { path: 'users', component: UsersComponent },
  { path: 'user', component: UserComponent },
  { path: 'user/:id', component: UserComponent },
  { path: 'add-user', component: AddUserComponent },
  { path: 'update-user', component: UpdateUserComponent },
  { path: 'update-user/:id', component: UpdateUserComponent },
  { path: 'delete-user/:id', redirectTo: '/users' },
  { path: 'login', component: LoginComponent },
  
  // Role routes
  { path: 'roles', component: RolesComponent },
  { path: 'role', component: RoleComponent },
  { path: 'role/:id', component: RoleComponent },
  { path: 'add-role', component: AddRoleComponent },
  { path: 'update-role', component: UpdateRoleComponent },
  { path: 'update-role/:id', component: UpdateRoleComponent },
  { path: 'delete-role/:id', redirectTo: '/roles' },
  
  // Wildcard route
  { path: '**', redirectTo: '' }
];
