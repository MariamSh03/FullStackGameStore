import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { filter } from 'rxjs/operators';

export interface ApiConfig {
  baseApiUrl: string;
  gamesAllApiUrl: string;
  getGameFileApiUrl: string;
  getPublishedDateOptionsApiUrl: string;
  getSortingOptionsApiUrl: string;
  getPaggingOptionsApiUrl: string;
  gameApiUrl: string;
  gameByIdApiUrl: string;
  gamesApiUrl: string;
  gamesByGenreApiUrl: string;
  gamesByPublisherApiUrl: string;
  gamesByPlatformApiUrl: string;
  updateGameApiUrl: string;
  addGameApiUrl: string;
  deleteGameApiUrl: string;
  genreApiUrl: string;
  genresApiUrl: string;
  genresByGameApiUrl: string;
  genresByParentApiUrl: string;
  updateGenreApiUrl: string;
  addGenreApiUrl: string;
  deleteGenreApiUrl: string;
  platformApiUrl: string;
  platformsApiUrl: string;
  platformsByGameApiUrl: string;
  updatePlatformApiUrl: string;
  addPlatformApiUrl: string;
  deletePlatformApiUrl: string;
  updateOrderDetailCountApiUrl: string;
  deleteOrderDetailApiUrl: string;
  addOrderDetailApiUrl: string;
  shipOrderApiUrl: string;
  userApiUrl: string;
  usersApiUrl: string;
  addUserApiUrl: string;
  updateUserApiUrl: string;
  deleteUserApiUrl: string;
  loginApiUrl: string;
  checkAccessApiUrl: string;
  roleApiUrl: string;
  rolesApiUrl: string;
  addRoleApiUrl: string;
  updateRoleApiUrl: string;
  deleteRoleApiUrl: string;
  userRolesApiUrl: string;
  rolePermissionsApiUrl: string;
  permissionsApiUrl: string;
  publisherApiUrl: string;
  publishersApiUrl: string;
  publisherByGameApiUrl: string;
  addPublisherApiUrl: string;
  deletePublisherApiUrl: string;
  updatePublisherApiUrl: string;
  historyApiUrl: string;
  orderApiUrl: string;
  ordersApiUrl: string;
  orderDetailsApiUrl: string;
  buyGameApiUrl: string;
  cancelGameBuyApiUrl: string;
  basketApiUrl: string;
  makeOrderInfoApiUrl: string;
  payApiUrl: string;
  addCommentApiUrl: string;
  commentsByGameApiUrl: string;
  deleteCommentApiUrl: string;
  getBanDurationsApiUrl: string;
  banUserApiUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiConfigService {
  private configSubject = new BehaviorSubject<ApiConfig | null>(null);
  public config$ = this.configSubject.asObservable();
  private configLoaded = false;

  constructor(private http: HttpClient) {
    // Initialize with default config immediately to ensure services don't wait indefinitely
    this.initializeDefaultConfig();
    this.loadConfiguration();
  }

  private initializeDefaultConfig(): void {
    const defaultConfig: ApiConfig = {
      baseApiUrl: 'https://localhost:7281/',
      gamesAllApiUrl: 'games/all',
      getGameFileApiUrl: 'games/{key}/file',
      getPublishedDateOptionsApiUrl: 'games/publish-date-options',
      getSortingOptionsApiUrl: 'games/sorting-options',
      getPaggingOptionsApiUrl: 'games/pagination-options',
      gameApiUrl: 'games/{key}',
      gameByIdApiUrl: 'games/find/{id}',
      gamesApiUrl: 'games',
      gamesByGenreApiUrl: 'genres/{id}/games',
      gamesByPublisherApiUrl: 'publishers/{companyname}/games',
      gamesByPlatformApiUrl: 'platforms/{id}/games',
      updateGameApiUrl: 'games',
      addGameApiUrl: 'games',
      deleteGameApiUrl: 'games/{key}',
      genreApiUrl: 'genres/{id}',
      genresApiUrl: 'genres',
      genresByGameApiUrl: 'games/{key}/genres',
      genresByParentApiUrl: 'genres/{id}/genres',
      updateGenreApiUrl: 'genres',
      addGenreApiUrl: 'genres',
      deleteGenreApiUrl: 'genres/{id}',
      platformApiUrl: 'platforms/{id}',
      platformsApiUrl: 'platforms',
      platformsByGameApiUrl: 'games/{key}/platforms',
      updatePlatformApiUrl: 'platforms',
      addPlatformApiUrl: 'platforms',
      deletePlatformApiUrl: 'platforms/{id}',
      updateOrderDetailCountApiUrl: 'orders/details/{id}/quantity',
      deleteOrderDetailApiUrl: 'orders/details/{id}',
      addOrderDetailApiUrl: 'orders/{id}/details/{key}',
      shipOrderApiUrl: 'orders/{id}/ship',
      userApiUrl: 'users/{id}',
      usersApiUrl: 'users',
      addUserApiUrl: 'users',
      updateUserApiUrl: 'users',
      deleteUserApiUrl: 'users/{id}',
      loginApiUrl: 'users/login',
      checkAccessApiUrl: 'users/access',
      roleApiUrl: 'roles/{id}',
      rolesApiUrl: 'roles',
      addRoleApiUrl: 'roles',
      updateRoleApiUrl: 'roles',
      deleteRoleApiUrl: 'roles/{id}',
      userRolesApiUrl: 'users/{id}/roles',
      rolePermissionsApiUrl: 'roles/{id}/permissions',
      permissionsApiUrl: 'roles/permissions',
      publisherApiUrl: 'publishers/{companyname}',
      publishersApiUrl: 'publishers',
      publisherByGameApiUrl: 'games/{key}/publisher',
      addPublisherApiUrl: 'publishers',
      deletePublisherApiUrl: 'publishers/{id}',
      updatePublisherApiUrl: 'publishers',
      historyApiUrl: 'orders/history',
      orderApiUrl: 'orders/{id}',
      ordersApiUrl: 'orders',
      orderDetailsApiUrl: 'orders/{id}/details',
      buyGameApiUrl: 'games/{key}/buy',
      cancelGameBuyApiUrl: 'orders/cart/{key}',
      basketApiUrl: 'orders/cart',
      makeOrderInfoApiUrl: 'orders/payment-methods',
      payApiUrl: 'orders/payment',
      addCommentApiUrl: 'games/{key}/comments',
      commentsByGameApiUrl: 'games/{key}/comments',
      deleteCommentApiUrl: 'games/{key}/comments/{id}',
      getBanDurationsApiUrl: 'comments/ban/durations',
      banUserApiUrl: 'comments/ban'
    };
    
    console.log('🔧 Initializing with default API configuration...');
    this.configSubject.next(defaultConfig);
    this.configLoaded = true;
  }

  private async loadConfiguration(): Promise<void> {
    try {
      console.log('🔄 Attempting to load custom API configuration from /assets/configuration.json...');
      const config = await this.http.get<ApiConfig>('/assets/configuration.json').toPromise();
      console.log('✅ Custom API configuration loaded successfully:', config);
      this.configSubject.next(config!);
    } catch (error) {
      console.log('⚠️ Custom configuration not found, using default configuration');
      // Default config is already loaded, so we don't need to do anything
    }
  }

  getConfig(): ApiConfig | null {
    return this.configSubject.value;
  }

  isConfigLoaded(): boolean {
    return this.configLoaded;
  }

  waitForConfig(): Observable<ApiConfig> {
    return this.config$.pipe(
      filter(config => config !== null)
    );
  }

  getBaseUrl(): string {
    const config = this.getConfig();
    return config?.baseApiUrl || 'https://localhost:7281/';
  }

  buildUrl(endpoint: string, params?: { [key: string]: string | number }): string {
    const config = this.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    let url = config.baseApiUrl + endpoint;
    
    if (params) {
      Object.keys(params).forEach(key => {
        const placeholder = `{${key}}`;
        url = url.replace(placeholder, params[key].toString());
      });
    }
    
    return url;
  }
}
