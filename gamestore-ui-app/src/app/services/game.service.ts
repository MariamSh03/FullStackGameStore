import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { filter, take, switchMap, map } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';

export interface Game {
  id: string; // Changed to string to match backend Guid
  key?: string;
  name: string; // Changed from title to match backend
  price: number;
  discount: number;
  unitInStock?: number; // Added to match backend
  description?: string;
  // Optional fields that might not come from backend
  rating?: number;
  reviewCount?: number;
  platform?: string;
  genre?: string;
  publisher?: string;
  images?: string[];
  publishedDate?: string;
}

export interface GameSearchParams {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  genreId?: number;
  publisherId?: number;
  platformId?: number;
  minPrice?: number;
  maxPrice?: number;
  searchQuery?: string;
}

export interface GameResponse {
  data: Game[];
  totalCount: number;
  currentPage: number;
  totalPages: number;
}

@Injectable({
  providedIn: 'root'
})
export class GameService {
  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {}

  getAllGames(params?: GameSearchParams): Observable<GameResponse> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.gamesAllApiUrl);
        let httpParams = new HttpParams();

        if (params) {
          Object.keys(params).forEach(key => {
            const value = params[key as keyof GameSearchParams];
            if (value !== undefined && value !== null) {
              httpParams = httpParams.set(key, value.toString());
            }
          });
        }

        // Backend returns PagedGamesResultDto with proper pagination info
        return this.http.get<any>(url, { params: httpParams }).pipe(
          switchMap(backendResponse => {
            // Transform backend response to match frontend GameResponse format
            const response: GameResponse = {
              data: backendResponse.games || backendResponse.Games || [],
              totalCount: backendResponse.totalCount || backendResponse.TotalCount || 0,
              currentPage: backendResponse.currentPage || backendResponse.CurrentPage || (params?.page || 1),
              totalPages: backendResponse.totalPages || backendResponse.TotalPages || 1
            };
            
            return of(response);
          })
        );
      })
    );
  }

  getGameById(id: string): Observable<Game> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.gameByIdApiUrl, { id });
        return this.http.get<Game>(url);
      })
    );
  }

  getGameByKey(key: string): Observable<Game> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.gameApiUrl, { key });
        return this.http.get<Game>(url);
      })
    );
  }

  getGamesByGenre(genreId: number, params?: GameSearchParams): Observable<GameResponse> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.gamesByGenreApiUrl, { id: genreId });
    let httpParams = new HttpParams();

    if (params) {
      Object.keys(params).forEach(key => {
        const value = params[key as keyof GameSearchParams];
        if (value !== undefined && value !== null) {
          httpParams = httpParams.set(key, value.toString());
        }
      });
    }

    return this.http.get<GameResponse>(url, { params: httpParams });
  }

  getGamesByPublisher(companyName: string, params?: GameSearchParams): Observable<GameResponse> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.gamesByPublisherApiUrl, { companyname: companyName });
    let httpParams = new HttpParams();

    if (params) {
      Object.keys(params).forEach(key => {
        const value = params[key as keyof GameSearchParams];
        if (value !== undefined && value !== null) {
          httpParams = httpParams.set(key, value.toString());
        }
      });
    }

    return this.http.get<GameResponse>(url, { params: httpParams });
  }

  getGamesByPlatform(platformId: number, params?: GameSearchParams): Observable<GameResponse> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.gamesByPlatformApiUrl, { id: platformId });
    let httpParams = new HttpParams();

    if (params) {
      Object.keys(params).forEach(key => {
        const value = params[key as keyof GameSearchParams];
        if (value !== undefined && value !== null) {
          httpParams = httpParams.set(key, value.toString());
        }
      });
    }

    return this.http.get<GameResponse>(url, { params: httpParams });
  }

  addGame(game: Partial<Game>): Observable<Game> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not available');
    }

    const url = this.apiConfig.buildUrl(config.addGameApiUrl);
    return this.http.post<Game>(url, game);
  }

  updateGame(game: Game): Observable<Game> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not available');
    }

    const url = this.apiConfig.buildUrl(config.updateGameApiUrl);
    return this.http.put<Game>(url, game);
  }

  deleteGame(key: string): Observable<void> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not available');
    }

    const url = this.apiConfig.buildUrl(config.deleteGameApiUrl, { key });
    return this.http.delete<void>(url);
  }

  getSortingOptions(): Observable<any[]> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.getSortingOptionsApiUrl);
    return this.http.get<any[]>(url);
  }
}
